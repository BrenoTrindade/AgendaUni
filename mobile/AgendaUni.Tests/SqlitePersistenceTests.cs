using AgendaUni.Models;
using AgendaUni.Repositories;
using AgendaUni.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgendaUni.Tests;

public class SqlitePersistenceTests
{
    [Fact]
    public async Task FreshDatabase_LoadsNotificationIdsAndDeletesClassWithDependents()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
        var course = new Class
        {
            ClassName = "Engenharia de Software",
            Absences = new List<Absence> { new() { AbsenceReason = "Compromisso pessoal" } },
            Schedules = new List<ClassSchedule> { new() { NotificationId = 31 } },
            Events = new List<Event>
            {
                new()
                {
                    Description = "Entrega do projeto",
                    EventNotifications = new List<EventNotification> { new() { NotificationId = 21 } }
                }
            }
        };
        context.Classes.Add(course);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        var notifications = new Mock<INotificationService>();
        var service = new ClassService(new ClassRepository(context), notifications.Object);

        var result = await service.DeleteClassAsync(course.Id);

        Assert.True(result.IsSuccess);
        notifications.Verify(item => item.CancelNotification(21), Times.Once);
        notifications.Verify(item => item.CancelNotification(31), Times.Once);
        Assert.Empty(await context.Classes.ToListAsync());
        Assert.Empty(await context.Absences.ToListAsync());
        Assert.Empty(await context.ClassSchedules.ToListAsync());
        Assert.Empty(await context.Events.ToListAsync());
        Assert.Empty(await context.EventNotifications.ToListAsync());
    }

    [Fact]
    public async Task ReplacingEventNotifications_PersistsOnlyNewIds()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
        var course = new Class { ClassName = "Engenharia de Software" };
        context.Classes.Add(course);
        await context.SaveChangesAsync();
        var repository = new EventRepository(context);
        var ev = new Event { ClassId = course.Id, Description = "Entrega do projeto" };
        await repository.AddAsync(ev, new[] { 21, 22 });

        await repository.UpdateAsync(ev, new[] { 41, 42 });
        context.ChangeTracker.Clear();
        var saved = await repository.GetByIdAsync(ev.Id);

        Assert.NotNull(saved);
        Assert.Equal(new[] { 41, 42 }, saved.EventNotifications.Select(item => item.NotificationId).Order());
        Assert.Equal(2, await context.EventNotifications.CountAsync());
    }
}

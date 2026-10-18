using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.Services;
using Moq;

namespace AgendaUni.Tests;

public class NotificationLifecycleTests
{
    [Fact]
    public async Task DeletingClass_CancelsEventAndScheduleNotificationsBeforeDeletingData()
    {
        var repository = new Mock<IClassRepository>();
        var notifications = new Mock<INotificationService>();
        var course = new Class
        {
            Id = 7,
            Events = new List<Event>
            {
                new() { EventNotifications = new List<EventNotification>
                    { new() { NotificationId = 21 }, new() { NotificationId = 22 } } }
            },
            Schedules = new List<ClassSchedule>
                { new() { NotificationId = 31 }, new() { NotificationId = null } }
        };
        repository.Setup(repo => repo.GetByIdAsync(7)).ReturnsAsync(course);
        repository.Setup(repo => repo.DeleteAsync(7)).Callback(() =>
        {
            notifications.Verify(service => service.CancelNotification(21), Times.Once);
            notifications.Verify(service => service.CancelNotification(22), Times.Once);
            notifications.Verify(service => service.CancelNotification(31), Times.Once);
        }).Returns(Task.CompletedTask);

        var result = await new ClassService(repository.Object, notifications.Object).DeleteClassAsync(7);

        Assert.True(result.IsSuccess);
        repository.Verify(repo => repo.DeleteAsync(7), Times.Once);
        notifications.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreatingEvent_WithMissingClassDoesNotScheduleOrSave()
    {
        var events = new Mock<IEventRepository>(MockBehavior.Strict);
        var classes = new Mock<IClassRepository>();
        var notifications = new Mock<INotificationService>(MockBehavior.Strict);
        var service = new EventService(events.Object, classes.Object, notifications.Object);

        var result = await service.AddEventAsync(new Event { ClassId = 7, Description = "Prova" });

        Assert.False(result.IsSuccess);
        events.VerifyNoOtherCalls();
        notifications.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RejectedEventEdit_PreservesExistingNotifications()
    {
        var events = new Mock<IEventRepository>();
        events.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new Event
        {
            Id = 1,
            EventNotifications = new List<EventNotification> { new() { NotificationId = 21 } }
        });
        var classes = new Mock<IClassRepository>();
        var notifications = new Mock<INotificationService>(MockBehavior.Strict);
        var service = new EventService(events.Object, classes.Object, notifications.Object);

        var result = await service.UpdateEventAsync(new Event { Id = 1, ClassId = 7, Description = "Prova" });

        Assert.False(result.IsSuccess);
        events.Verify(repo => repo.UpdateAsync(It.IsAny<Event>(), It.IsAny<IEnumerable<int>>()), Times.Never);
        notifications.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RejectedScheduleEdit_PreservesExistingNotification()
    {
        var schedules = new Mock<IClassScheduleRepository>();
        schedules.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ClassSchedule { Id = 1, NotificationId = 31 });
        var classes = new Mock<IClassRepository>();
        var notifications = new Mock<INotificationService>(MockBehavior.Strict);
        var service = new ClassScheduleService(schedules.Object, classes.Object, notifications.Object);

        var result = await service.UpdateClassScheduleAsync(new ClassSchedule
            { Id = 1, ClassId = 7, DayOfWeek = DayOfWeek.Monday, ClassTime = TimeSpan.FromHours(19) });

        Assert.False(result.IsSuccess);
        schedules.Verify(repo => repo.UpdateAsync(It.IsAny<ClassSchedule>()), Times.Never);
        notifications.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreatingSchedule_WithNotificationsDisabledDoesNotStoreSentinelId()
    {
        var schedules = new Mock<IClassScheduleRepository>();
        var classes = new Mock<IClassRepository>();
        classes.Setup(repo => repo.GetByIdAsync(7)).ReturnsAsync(new Class { Id = 7 });
        var notifications = new Mock<INotificationService>();
        notifications.Setup(service => service.ScheduleNotificationForClassSchedule(
            It.IsAny<ClassSchedule>(), It.IsAny<Class>())).ReturnsAsync(-1);
        var schedule = new ClassSchedule
            { ClassId = 7, DayOfWeek = DayOfWeek.Monday, ClassTime = TimeSpan.FromHours(19) };

        var result = await new ClassScheduleService(schedules.Object, classes.Object, notifications.Object)
            .AddClassScheduleAsync(schedule);

        Assert.True(result.IsSuccess);
        Assert.Null(schedule.NotificationId);
        schedules.Verify(repo => repo.AddAsync(schedule), Times.Once);
    }
}

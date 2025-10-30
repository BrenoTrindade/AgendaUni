using AgendaUni.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Absence> Absences { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassSchedule> ClassSchedules { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventNotification> EventNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>().ToTable("Class");
        modelBuilder.Entity<Absence>().ToTable("Absence");
        modelBuilder.Entity<ClassSchedule>().ToTable("ClassSchedule");
        modelBuilder.Entity<Event>().ToTable("Event");
        modelBuilder.Entity<EventNotification>().ToTable("EventNotification");

        modelBuilder.Entity<Class>()
            .HasMany(c => c.Absences)
            .WithOne(a => a.Class)
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Class>()
            .HasMany(c => c.Schedules)
             .WithOne(s => s.Class)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Class>()
            .HasMany(c => c.Events)
            .WithOne(e => e.Class)
            .HasForeignKey(e => e.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Event>()
            .HasMany(c => c.EventNotifications)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

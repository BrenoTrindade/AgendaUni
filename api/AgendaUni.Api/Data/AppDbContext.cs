using AgendaUni.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaUni.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Absence> Absences { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Absences)
                .WithOne()
                .HasForeignKey(a => a.ClassId);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Schedules)
                .WithOne()
                .HasForeignKey(cs => cs.ClassId);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Events)
                .WithOne()
                .HasForeignKey(e => e.ClassId);
        }
    }
}
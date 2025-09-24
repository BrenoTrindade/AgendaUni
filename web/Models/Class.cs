using System.ComponentModel.DataAnnotations;

namespace AgendaUni.Web.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; }

        [Required]
        public int MaximumAbsences { get; set; }

        public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

        public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();

        public virtual ICollection<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();
    }
}
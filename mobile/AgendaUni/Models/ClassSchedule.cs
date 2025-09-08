using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaUni.Models
{
    public class ClassSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan ClassTime { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
    }
}

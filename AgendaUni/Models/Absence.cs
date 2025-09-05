using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaUni.Models
{
    public class Absence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime AbsenceDate { get; set; }

        [Required]
        [StringLength(200)]
        public string AbsenceReason { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
    }
}

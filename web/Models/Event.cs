using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaUni.Web.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime EventDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
    }
}
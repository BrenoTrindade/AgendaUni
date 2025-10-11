using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaUni.Models
{
    public class EventNotification
    {
        [Key]
        public int Id { get; set; }

        public int NotificationId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}

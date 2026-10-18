using System.ComponentModel.DataAnnotations;

namespace AgendaUni.Models;
public class Event
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<EventNotification> EventNotifications { get; set; } = new List<EventNotification>();
}

using AgendaUni.Models;

namespace AgendaUni.Services;

public interface INotificationService
{
    Task<List<int>> ScheduleNotificationForEvent(Event ev, Class cl);
    Task<int> ScheduleNotificationForClassSchedule(ClassSchedule schedule, Class cl);
    void CancelNotification(int notificationId);
}

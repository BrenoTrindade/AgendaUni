using AgendaUni.Services;

namespace AgendaUni.ViewModels;

public class NotificationsViewModel : BaseViewModel
{
    private readonly NotificationService _notificationService;
    private bool _eventNotificationsEnabled;
    private bool _classScheduleNotificationsEnabled;

    public bool EventNotificationsEnabled
    {
        get => _eventNotificationsEnabled;
        set
        {
            if (SetProperty(ref _eventNotificationsEnabled, value))
            {
                Preferences.Set("IsEventNotificationEnabled", value);
                if (value)
                {
                    _notificationService.RescheduleEventNotifications();
                }
                else
                {
                    _notificationService.CancelNotificationsByType("event");
                }
            }
        }
    }

    public bool ClassScheduleNotificationsEnabled
    {
        get => _classScheduleNotificationsEnabled;
        set
        {
            if (SetProperty(ref _classScheduleNotificationsEnabled, value))
            {
                Preferences.Set("IsClassScheduleNotificationEnabled", value);
                if (value)
                {
                    _notificationService.RescheduleClassScheduleNotifications();
                }
                else
                {
                    _notificationService.CancelNotificationsByType("class_schedule");
                }
            }
        }
    }

    public NotificationsViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        Title = "Notificações";
        _eventNotificationsEnabled = Preferences.Get("IsEventNotificationEnabled", true);
        _classScheduleNotificationsEnabled = Preferences.Get("IsClassScheduleNotificationEnabled", true);
    }
}

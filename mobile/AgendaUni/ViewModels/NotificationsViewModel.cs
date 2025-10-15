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
                _notificationService.IsEventNotificationEnabled = value;
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
                _notificationService.IsClassScheduleNotificationEnabled = value;
            }
        }
    }

    public NotificationsViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        Title = "Notificações";
        _eventNotificationsEnabled = _notificationService.IsEventNotificationEnabled;
        _classScheduleNotificationsEnabled = _notificationService.IsClassScheduleNotificationEnabled;
    }
}

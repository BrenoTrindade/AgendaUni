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
        private set => SetProperty(ref _eventNotificationsEnabled, value);
    }

    public bool ClassScheduleNotificationsEnabled
    {
        get => _classScheduleNotificationsEnabled;
        private set => SetProperty(ref _classScheduleNotificationsEnabled, value);
    }

    public bool CanChangeNotifications => !IsBusy;

    public async Task SetEventNotificationsEnabledAsync(bool enabled)
    {
        if (IsBusy || enabled == EventNotificationsEnabled) return;
        await ChangePreferenceAsync("IsEventNotificationEnabled", enabled,
            enabled ? _notificationService.RescheduleEventNotifications
                : () => _notificationService.CancelNotificationsByType("event"));
        EventNotificationsEnabled = enabled;
    }

    public async Task SetClassScheduleNotificationsEnabledAsync(bool enabled)
    {
        if (IsBusy || enabled == ClassScheduleNotificationsEnabled) return;
        await ChangePreferenceAsync("IsClassScheduleNotificationEnabled", enabled,
            enabled ? _notificationService.RescheduleClassScheduleNotifications
                : () => _notificationService.CancelNotificationsByType("class_schedule"));
        ClassScheduleNotificationsEnabled = enabled;
    }

    private async Task ChangePreferenceAsync(string key, bool enabled, Func<Task> apply)
    {
        var previous = Preferences.Get(key, true);
        IsBusy = true;
        OnPropertyChanged(nameof(CanChangeNotifications));
        try
        {
            Preferences.Set(key, enabled);
            await apply();
        }
        catch
        {
            Preferences.Set(key, previous);
            throw;
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanChangeNotifications));
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

using AgendaUni.Services;
using System.Windows.Input;

namespace AgendaUni.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ThemeService _themeService;
        private bool _isDarkModeEnabled;

        public bool IsDarkModeEnabled
        {
            get => _isDarkModeEnabled;
            set
            {
                if (SetProperty(ref _isDarkModeEnabled, value))
                {
                    _themeService.SetTheme(value);
                }
            }
        }

        public ICommand GoToNotificationsCommand { get; }

        public SettingsViewModel(ThemeService themeService)
        {
            _themeService = themeService;
            Title = "Configurações";
            _isDarkModeEnabled = _themeService.IsDarkMode;

            GoToNotificationsCommand = new Command(async () => await GoToNotifications());
        }

        async Task GoToNotifications()
        {
            await Shell.Current.GoToAsync("NotificationsPage");
        }
    }
}

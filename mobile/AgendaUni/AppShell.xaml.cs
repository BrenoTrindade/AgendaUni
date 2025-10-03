using AgendaUni.Services;
using AgendaUni.Views;

namespace AgendaUni
{
    public partial class AppShell : Shell
    {
        private bool _isDarkMode;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ClassPage), typeof(ClassPage));
            Routing.RegisterRoute(nameof(ClassListPage), typeof(ClassListPage));
            Routing.RegisterRoute(nameof(AbsencePage), typeof(AbsencePage));
            Routing.RegisterRoute(nameof(AbsenceListPage), typeof(AbsenceListPage));
            Routing.RegisterRoute(nameof(ClassSchedulePage), typeof(ClassSchedulePage));
            Routing.RegisterRoute(nameof(ClassScheduleListPage), typeof(ClassScheduleListPage));
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(EventListPage), typeof(EventListPage));
            _isDarkMode = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void OnThemeClicked(object sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            Application.Current.UserAppTheme = _isDarkMode ? AppTheme.Dark : AppTheme.Light;
        }
    }
}

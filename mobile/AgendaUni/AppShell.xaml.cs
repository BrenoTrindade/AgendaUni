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
            Routing.RegisterRoute(nameof(ClassSchedulePage), typeof(ClassSchedulePage));
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
        }
    }
}

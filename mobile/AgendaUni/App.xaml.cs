using Microsoft.EntityFrameworkCore;
using Plugin.LocalNotification;

namespace AgendaUni
{
    public partial class App : Application
    {
        public App(AppDbContext dbContext)
        {
            InitializeComponent();
            Current.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
        }
    }
}
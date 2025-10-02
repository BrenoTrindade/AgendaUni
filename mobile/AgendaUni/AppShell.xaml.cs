using AgendaUni.Services;

namespace AgendaUni
{
    public partial class AppShell : Shell
    {
        private bool _isDarkMode;
        public AppShell()
        {
            InitializeComponent();
            _isDarkMode = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void OnThemeClicked(object sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            Application.Current.UserAppTheme = _isDarkMode ? AppTheme.Dark : AppTheme.Light;
        }
    }
}

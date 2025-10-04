using System;

namespace AgendaUni.Services
{
    public class ThemeService
    {
        public void SetTheme(bool isDarkMode)
        {
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
            }
        }
    }
}

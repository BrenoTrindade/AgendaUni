using System;

namespace AgendaUni.Services;
public class ThemeService
{
    private const string IsDarkModeKey = "is_dark_mode";
    public bool IsDarkMode
    {
        get => Application.Current?.UserAppTheme == AppTheme.Dark;
        set => SetTheme(value);
    }

    public void InitializeTheme()
    {
        var isDarkMode = Preferences.Get(IsDarkModeKey, false);
        SetTheme(isDarkMode);
    }

    public void SetTheme(bool isDarkMode)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
            Preferences.Set(IsDarkModeKey, isDarkMode);
        }
    }
}

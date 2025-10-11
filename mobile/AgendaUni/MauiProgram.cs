using Microsoft.Extensions.Logging;
using AgendaUni.Services;
using AgendaUni.Repositories;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.ViewModels;
using AgendaUni.Views;

using Plugin.LocalNotification;

namespace AgendaUni
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "absence.db");
            builder.Services.AddScoped(sp => new AppDbContext(dbPath));

            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<ClassService>();

            builder.Services.AddScoped<IAbsenceRepository, AbsenceRepository>();
            builder.Services.AddScoped<AbsenceService>();

            builder.Services.AddScoped<IClassScheduleRepository, ClassScheduleRepository>();
            builder.Services.AddScoped<ClassScheduleService>();

            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<EventService>();

            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddScoped<NotificationService>();


            builder.Services.AddTransient<ClassViewModel>();
            builder.Services.AddTransient<ClassPage>();

            builder.Services.AddTransient<ClassListViewModel>();
            builder.Services.AddTransient<ClassListPage>();

            builder.Services.AddTransient<ClassScheduleViewModel>();
            builder.Services.AddTransient<ClassSchedulePage>();

            builder.Services.AddTransient<AbsenceViewModel>();
            builder.Services.AddTransient<AbsencePage>();


            builder.Services.AddTransient<EventViewModel>();
            builder.Services.AddTransient<EventPage>();

            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            return app;
        }
    }
}

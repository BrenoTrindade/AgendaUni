using System.Globalization;

namespace AgendaUni.Common.Converters
{
    public class EventsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AgendaUni.ViewModels.MainPageViewModel.EventType eventType)
            {
                switch (eventType)
                {
                    case AgendaUni.ViewModels.MainPageViewModel.EventType.Absence:
                        return Color.FromArgb("#8B0000");

                    case AgendaUni.ViewModels.MainPageViewModel.EventType.ClassSchedule:
                        return Color.FromArgb("#2F4F4F");

                    case AgendaUni.ViewModels.MainPageViewModel.EventType.Event:
                        return Color.FromArgb("#556B2F");

                    default:
                        return Color.FromArgb("#A9A9A9");
                }

            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
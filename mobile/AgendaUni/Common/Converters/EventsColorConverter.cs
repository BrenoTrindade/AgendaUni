using AgendaUni.Common.Enums;
using System.Globalization;

namespace AgendaUni.Common.Converters
{
    public class EventsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EventType eventType)
            {
                switch (eventType)
                {
                    case EventType.Absence:
                        return Color.FromArgb("#8B0000");

                    case EventType.ClassSchedule:
                        return Color.FromArgb("#2F4F4F");

                    case EventType.Event:
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
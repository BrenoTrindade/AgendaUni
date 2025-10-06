using System.Globalization;

namespace AgendaUni.Common.Converters
{
    public class GreaterThanOrEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IComparable comparableValue || parameter == null)
                return false;

            var parameterValue = System.Convert.ToDouble(parameter, culture);
            var comparableAsDouble = System.Convert.ToDouble(comparableValue, culture);

            return comparableAsDouble >= parameterValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

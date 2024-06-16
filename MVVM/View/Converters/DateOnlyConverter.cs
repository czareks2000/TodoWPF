using System.Globalization;
using System.Windows.Data;

namespace Todo.MVVM.View.Converters
{
    public class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("d", culture); // "d" format zwraca tylko datę w zależności od ustawień kulturowych
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                if (DateTime.TryParseExact(strValue, "d", culture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            return value;
        }
    }
}

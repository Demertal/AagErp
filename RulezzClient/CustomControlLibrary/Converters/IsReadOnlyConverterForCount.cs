using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class IsReadOnlyConverterForCount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return value.ToString() != "Нет";
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

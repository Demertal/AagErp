using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class IsReadOnlyConverterForSerialNumber : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] is int i && values[0] is string)
            {
                if (values[0].ToString() == "Нет") return true;
                if (i == 0) return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
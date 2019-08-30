using System;
using System.Globalization;
using System.Windows.Data;

namespace ModelModul.Converters
{
    public class CalculationSum : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is decimal price) || !(values[1] is decimal count))
                return 0;
            return price * count;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

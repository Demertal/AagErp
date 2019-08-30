using System;
using System.Globalization;
using System.Windows.Data;

namespace ModelModul.Converters
{
    public class ConvertCount: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is decimal count) || !(values[1] is bool isWeight)) return 0.ToString("F2");
            return isWeight ? count.ToString("F2") : ((int)count).ToString("D");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (int.TryParse((string) value, out var intResult))
            {
                return new object[] {(decimal)intResult };
            }

            return decimal.TryParse((string)value, out var decimalResult) ? new object[] { decimalResult } : new object[] { 0 };
        }
    }
}

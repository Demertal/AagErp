using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class ConvertCount: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double count) || !(values[1] is bool isWeight)) return 0.ToString("F2");
            return isWeight ? count.ToString("F2") : ((int)count).ToString("D");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (int.TryParse((string) value, out var intResult))
            {
                return new object[] {(double)intResult };
            }

            return double.TryParse((string)value, out var doubleResult) ? new object[] {doubleResult} : new object[] { 0 };
        }
    }
}

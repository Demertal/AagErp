using System;
using System.Globalization;
using System.Windows.Data;

namespace PriceGroupModul.Converters
{
    public class MarkupConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal val) return (val * 100).ToString("F2", culture.NumberFormat);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && decimal.TryParse(s, NumberStyles.Any, culture.NumberFormat, out var val)) return val / 100;
            return null;
        }
    }
}

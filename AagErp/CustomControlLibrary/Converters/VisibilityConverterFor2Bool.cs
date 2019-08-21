using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class VisibilityConverterFor2Bool : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool v1)
            {
                if (v1) return Visibility.Collapsed;
                if (values[1] is bool v2)
                {
                    if (v2) return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

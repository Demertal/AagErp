using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class VisibilityConverterForNullAndBool: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool) values[1]) return Visibility.Collapsed;
            if (parameter != null && parameter.ToString() == "InvertValue1")
            {
                return values[0] == null ? Visibility.Collapsed : Visibility.Visible;
            }
            return values[0] == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

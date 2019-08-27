using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class VisibilityBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string)) return Visibility.Collapsed;
            if (parameter.ToString() == bool.TrueString)
            {
                if (value is bool val)
                {
                    return val ? Visibility.Collapsed : Visibility.Visible;
                }

                return Visibility.Visible;
            }
            else
            {
                if (value is bool val)
                {
                    return val ? Visibility.Visible : Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

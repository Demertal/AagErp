using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visibility = false;
            if (value is string)
            {
                visibility = value.ToString() == "Нет";
            }
            return visibility ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

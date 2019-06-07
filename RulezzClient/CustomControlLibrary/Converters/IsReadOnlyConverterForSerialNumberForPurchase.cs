using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class IsReadOnlyConverterForSerialNumberForPurchase: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return !string.IsNullOrEmpty(value.ToString());
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

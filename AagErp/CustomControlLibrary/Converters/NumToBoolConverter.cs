using System;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class NumToBoolConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is int) && !(value is long)) return null;
            return value is int i && i != 0 || value is long && (long)value != 0;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool)) return null;
            var val = (bool)value;
            return val ? 1 : 0;
        }

        #endregion
    }
}

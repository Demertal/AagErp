using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class CompareObjectConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = values[0]?.GetType();
            if (type == null || values.Any(value => value == null || type != value.GetType())) return null;
            if (type == typeof(int))
            {
                int i = (int) values[0];
                return values.All(value => i == (int)value);
            }
            var val = System.Convert.ChangeType(values[0], type);
            return values.All(value => val == System.Convert.ChangeType(value, type));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

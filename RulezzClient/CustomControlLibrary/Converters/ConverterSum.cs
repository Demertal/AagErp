using System;
using System.Globalization;
using System.Windows.Data;
using ModelModul;

namespace CustomControlLibrary.Converters
{
    public class ConverterSum : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal sum = 0;
            if (!(values[0] is decimal val) || !(values[1] is int))
                return sum.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            sum = val * (int)values[1];
            if (values.Length == 2)
            {
                return sum.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            }

            if (values[2] is ExchangeRates exchange && values[3] is bool && values[4] is decimal)
            {
                if (exchange.Title == "ГРН")
                {
                    return sum.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
                }

                if (!(bool)values[3])
                {
                    return sum.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                }

                return ((decimal)values[4] * sum).ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            }
            sum = 0;
            return sum.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

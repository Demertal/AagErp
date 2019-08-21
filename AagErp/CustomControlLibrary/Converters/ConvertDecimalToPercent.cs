using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Converters
{
    public class ConvertDecimalToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = new CultureInfo("ru-RU")
            {
                NumberFormat =
                {
                    PercentDecimalSeparator = ".",
                    PercentSymbol = "%",
                    PercentGroupSeparator = ","
                }
            };

            return value;//((decimal) value).ToString("P1", cul);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = new CultureInfo("ru-RU")
            {
                NumberFormat =
                {
                    PercentDecimalSeparator = ".",
                    PercentSymbol = "%",
                    PercentGroupSeparator = ",",
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ","
                }
            };
            value = ((string)value).Remove(((string)value).IndexOf('%'));
            try
            {
                decimal result = System.Convert.ToDecimal(value, cul);
                return result;
            }
            catch
            {
                // ignored
            }

            return 0;
        }
    }
}

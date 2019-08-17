using System;
using System.Globalization;
using System.Windows.Data;
using ModelModul.Models;

namespace CustomControlLibrary.Converters
{
    public class CurrencyConverter4Parametrs : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is decimal val) || !(values[1] is Currency exchange)) return 0.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            if (exchange.Title == "ГРН")
            {
                return val.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            }

            if (!(bool) values[2])
            {
                return val.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            }
            return ((decimal)values[3] * val).ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            object[] param = new object[1];
            if (!(value is string varStr))
            {
                param[0] = 0;
                return param;
            }
            CultureInfo cul = new CultureInfo("ru-RU")
            {
                NumberFormat =
                {
                    CurrencySymbol = "₴",
                    CurrencyDecimalSeparator = ".",
                    CurrencyGroupSeparator = ","
                }
            };
            if (Decimal.TryParse(varStr, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, cul, out decimal val))
            {
                param[0] = val;
                return param;
            }

            cul.NumberFormat.CurrencySymbol = "$";
            if (Decimal.TryParse(varStr, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, cul, out val))
            {
                param[0] = val;
                return param;
            }
            param[0] = 0;
            return param;
        }
    }
}
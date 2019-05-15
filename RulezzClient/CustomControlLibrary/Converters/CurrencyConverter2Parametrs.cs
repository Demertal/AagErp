using System;
using System.Globalization;
using System.Windows.Data;
using ModelModul;

namespace CustomControlLibrary.Converters
{
    public class CurrencyConverter2Parametrs : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is decimal val) || !(values[1] is ExchangeRates exchange)) return 0.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            if (exchange.Title == "ГРН")
            {
                return val.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            }
            return val.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
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
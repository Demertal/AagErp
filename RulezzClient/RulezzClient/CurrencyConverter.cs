using System;
using System.Globalization;
using System.Windows.Data;

namespace RulezzClient
{
    class CurrencyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].GetType().ToString() == "RulezzClient.RevaluationProductModel")
            {
                if (!(values[0] is RevaluationProductModel valueAsItem)) return string.Empty;
                using (StoreEntities db = new StoreEntities())
                {
                    ExchangeRate exchange = db.ExchangeRate.Find(valueAsItem.IdExchangeRate);
                    if (exchange.Title == "грн" || exchange.Title == "ГРН")
                    {
                        return valueAsItem.PurchasePrice.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
                    }
                    if (!(bool)values[1])
                    {
                        return valueAsItem.PurchasePrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    return ((double)valueAsItem.PurchasePrice * exchange.Сourse).ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
                }
            }
            if (!(values[0] is decimal val)) return string.Empty;
            using (StoreEntities db = new StoreEntities())
            {
                ExchangeRate exchange = db.ExchangeRate.Find(values[1] as int?);
                if (exchange?.Title == "грн" || exchange?.Title == "ГРН")
                {
                    return val.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
                }
                return val.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            }
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
            if (Decimal.TryParse(varStr, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, cul, out decimal val)) { 
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

using System;
using System.Globalization;
using System.Windows.Data;

namespace RulezzClient
{
    class CurrencyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is RevaluationProductModel valueAsItem)) return string.Empty;
            decimal amount = valueAsItem.PurchasePrice;
            using (StoreEntities db = new StoreEntities())
            {
                ExchangeRate exchange = db.ExchangeRate.Find(valueAsItem.IdExchangeRate);
                if (exchange.Title == "грн" || exchange.Title == "ГРН")
                {
                    return valueAsItem.PurchasePrice.ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
                }
                if (!(bool) values[1])
                {
                    return valueAsItem.PurchasePrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                }
                return ((double)valueAsItem.PurchasePrice * exchange.Сourse).ToString("C", CultureInfo.CreateSpecificCulture("ua-UA"));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

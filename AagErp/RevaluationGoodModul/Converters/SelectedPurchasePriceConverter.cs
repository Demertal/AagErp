using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ModelModul.Models;

namespace RevaluationGoodModul.Converters
{
    public class SelectedPurchasePriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is ICollection<EquivalentCostForExistingProduct> equivalentCosts) ||
                !(values[1] is ICollection<Currency> currencies) || equivalentCosts.Count == 0) return 0;
            EquivalentCostForExistingProduct temp = equivalentCosts.OrderByDescending(e =>
                e.EquivalentCost * currencies.First(c => c.Id == e.EquivalentCurrencyId).Cost).FirstOrDefault();
            return temp?.EquivalentCost ?? 0;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

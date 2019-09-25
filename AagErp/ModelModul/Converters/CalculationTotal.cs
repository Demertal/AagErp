using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ModelModul.Models;

namespace ModelModul.Converters
{
    public class CalculationTotal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ICollection<MovementGoodsInfo> movementGoods:
                    return movementGoods.Sum(m => m.Price * m.Count);
                case ICollection<ProfitStatement> profitStatements:
                    return profitStatements.Sum(p => p.Summa);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

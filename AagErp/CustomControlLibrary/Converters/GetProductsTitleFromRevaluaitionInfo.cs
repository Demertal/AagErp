using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ModelModul.Models;

namespace CustomControlLibrary.Converters
{
    public class GetProductsTitleFromRevaluaitionInfo: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection<PriceProduct> priceProducts)) return "";
            string result = "";
            int count = 0;
            while (count < priceProducts.Count && count < 10)
            {
                result += priceProducts.ElementAt(count).Product.Title;
                if (count + 1 < priceProducts.Count && count + 1 < 10)
                {
                    result += " , ";
                }
                else if (count + 1 < priceProducts.Count &&  count + 1 == 10)
                {
                    result += "...";
                }

                count++;
            }

            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

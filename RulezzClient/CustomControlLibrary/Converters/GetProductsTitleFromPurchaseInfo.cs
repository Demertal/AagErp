using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ModelModul;

namespace CustomControlLibrary.Converters
{
    public class GetProductsTitleFromPurchaseInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection<PurchaseInfos> purchaseInfos)) return "";
            string result = "";
            int count = 0;
            while (count < purchaseInfos.Count && count < 10)
            {
                result += purchaseInfos.ElementAt(count).Products.Title;
                if (count + 1 < purchaseInfos.Count && count + 1 < 10)
                {
                    result += " , ";
                }
                else if (count + 1 < purchaseInfos.Count && count + 1 == 10)
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

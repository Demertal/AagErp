using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ModelModul;

namespace CustomControlLibrary.Converters
{
    public class GetProductsTitleFromSalesInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection<SalesInfos> salesInfos)) return "";
            string result = "";
            int count = 0;
            while (count < salesInfos.Count && count < 10)
            {
                result += salesInfos.ElementAt(count).Products.Title;
                if (count + 1 < salesInfos.Count && count + 1 < 10)
                {
                    result += " , ";
                }
                else if (count + 1 < salesInfos.Count && count + 1 == 10)
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

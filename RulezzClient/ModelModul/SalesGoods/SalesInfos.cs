using System;
using Prism.Mvvm;

namespace ModelModul
{
    public partial class SalesInfos: BindableBase, ICloneable
    {
        public object Clone()
        {
            return new SalesInfos
            {
                Id = Id,
                IdProduct = IdProduct,
                IdSalesReport = IdSalesReport,
                IdSerialNumber = IdSerialNumber,
                Count = Count,
                Products = (Products) Products.Clone(),
                SellingPrice = SellingPrice,
                SerialNumbers = (SerialNumbers)SerialNumbers.Clone()
            };
        }
    }
}

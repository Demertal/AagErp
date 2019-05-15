using System;
using System.Collections.Generic;

namespace ModelModul
{
    public partial class Products : ICloneable
    {
        public double Count
        {
            get
            {
                if (CountProducts == null || CountProducts.Count == 0) return 0;
                double count = 0;
                foreach (var ct in CountProducts)
                {
                    count += ct.Count;
                }
                return count;
            }
        }

        public object Clone()
        {
            List<SerialNumbers> temp = new List<SerialNumbers>(SerialNumbers);
            return new Products
            {
                Title = Title,
                VendorCode = VendorCode,
                Barcode = Barcode,
                PurchasePrice = PurchasePrice,
                SalesPrice = SalesPrice,
                IdExchangeRate = IdExchangeRate,
                IdGroup = IdGroup,
                IdWarrantyPeriod = IdWarrantyPeriod,
                IdUnitStorage = IdWarrantyPeriod,
                Id = Id,
                UnitStorages = (UnitStorages)UnitStorages?.Clone(),
                WarrantyPeriods = (WarrantyPeriods) WarrantyPeriods?.Clone(),
                Groups = (Groups) Groups?.Clone(),
                SerialNumbers = temp
            };
        }
    }
}

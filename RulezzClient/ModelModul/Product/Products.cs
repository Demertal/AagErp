using System;
using System.Collections.Generic;

namespace ModelModul
{
    public class PurchaseStruct
    {
        public decimal PurchasePrice;
        public ExchangeRates ExchangeRate;
    }

    public partial class Products : ICloneable
    {
        public object Clone()
        {
            List<SerialNumbers> temp = null;
            if (SerialNumbers != null)
            {
                temp = new List<SerialNumbers>(SerialNumbers);
            }
            return new Products
            {
                Title = Title,
                VendorCode = VendorCode,
                Barcode = Barcode,
                SalesPrice = SalesPrice,
                IdGroup = IdGroup,
                IdWarrantyPeriod = IdWarrantyPeriod,
                IdUnitStorage = IdUnitStorage,
                Id = Id,
                UnitStorages = (UnitStorages)UnitStorages?.Clone(),
                WarrantyPeriods = (WarrantyPeriods) WarrantyPeriods?.Clone(),
                Groups = (Groups) Groups?.Clone(),
                SerialNumbers = temp
            };
        }
    }
}

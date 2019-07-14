using System;

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
            return new Products
            {
                Title = Title,
                VendorCode = VendorCode,
                Barcode = Barcode,
                IdGroup = IdGroup,
                IdWarrantyPeriod = IdWarrantyPeriod,
                IdUnitStorage = IdUnitStorage,
                IdPriceGroup = IdPriceGroup,
                KeepTrackSerialNumbers = KeepTrackSerialNumbers,
                Id = Id,
                UnitStorages = (UnitStorages)UnitStorages?.Clone(),
                WarrantyPeriods = (WarrantyPeriods) WarrantyPeriods?.Clone(),
                Groups = (Groups) Groups?.Clone(),
                PriceGroups = PriceGroups
            };
        }
    }
}

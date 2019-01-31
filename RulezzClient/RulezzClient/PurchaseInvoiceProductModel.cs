using System;

namespace RulezzClient
{
    class PurchaseInvoiceProductModel : Product, IEquatable<PurchaseInvoiceProductModel>
    {
        public PurchaseInvoiceProductModel()
        {
            OldPurchasePrice = 0;
            Count = 0;
        }

        public PurchaseInvoiceProductModel(Product obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            VendorCode = obj.VendorCode;
            Barcode = obj.Barcode;
            Count = 0;
            PurchasePrice = obj.PurchasePrice;
            OldPurchasePrice = obj.PurchasePrice;
            SalesPrice = obj.SalesPrice;
            IdNomenclatureSubGroup = obj.IdNomenclatureSubGroup;
            IdUnitStorage = obj.IdUnitStorage;
            IdExchangeRate = obj.IdExchangeRate;
            IdWarrantyPeriod = obj.IdWarrantyPeriod;
        }

        public decimal OldPurchasePrice { get; set; }

        public bool Equals(PurchaseInvoiceProductModel obj)
        {
            return obj != null && Id == obj.Id && Title == obj.Title && VendorCode == obj.VendorCode &&
                   Barcode == obj.Barcode && OldPurchasePrice == obj.PurchasePrice &&
                   IdNomenclatureSubGroup == obj.IdNomenclatureSubGroup &&
                   IdUnitStorage == obj.IdUnitStorage && IdExchangeRate == obj.IdExchangeRate &&
                   IdWarrantyPeriod == obj.IdWarrantyPeriod;
        }
    }
}
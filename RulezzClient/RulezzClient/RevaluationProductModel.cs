using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulezzClient
{
    class RevaluationProductModel : Product, IEquatable<RevaluationProductModel>
    {
        public RevaluationProductModel()
        {
            OldSalesPrice = 0;
        }

        public RevaluationProductModel(Product obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            VendorCode = obj.VendorCode;
            Barcode = obj.Barcode;
            Count = obj.Count;
            PurchasePrice = obj.PurchasePrice;
            OldSalesPrice = obj.SalesPrice;
            SalesPrice = obj.SalesPrice;
            IdNomenclatureSubGroup = obj.IdNomenclatureSubGroup;
            IdUnitStorage = obj.IdUnitStorage;
            IdExchangeRate = obj.IdExchangeRate;
            IdWarrantyPeriod = obj.IdWarrantyPeriod;
        }

        public decimal OldSalesPrice { get; set; }

        public bool Equals(RevaluationProductModel obj)
        {
            return obj != null && Id == obj.Id && Title == obj.Title && VendorCode == obj.VendorCode &&
                   Barcode == obj.Barcode && Count == obj.Count &&
                   PurchasePrice == obj.PurchasePrice && OldSalesPrice == obj.SalesPrice &&
                   IdNomenclatureSubGroup == obj.IdNomenclatureSubGroup &&
                   IdUnitStorage == obj.IdUnitStorage && IdExchangeRate == obj.IdExchangeRate &&
                   IdWarrantyPeriod == obj.IdWarrantyPeriod;
        }
    }
}

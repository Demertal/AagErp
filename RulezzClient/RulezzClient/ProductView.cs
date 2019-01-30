using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulezzClient
{
    class ProductView
    {

        public ProductView(ProductView_Result obj)
        {
            this.Id = obj.Id;
            this.Barcode = obj.Barcode;
            this.Count = obj.Count;
            this.ExchangeRate = obj.ExchangeRate;
            this.UnitStorage = obj.UnitStorage;
            this.WarrantyPeriod = obj.WarrantyPeriod;
            this.PurchasePrice = obj.PurchasePrice;
            this.SalesPrice = obj.SalesPrice;
            this.Title = obj.Title;
            this.VendorCode = obj.VendorCode;
            this.IdNomenclatureSubGroup = obj.IdNomenclatureSubGroup;
        }

        public ProductView()
        {

        }

        public int Id { get; set; }
        public int IdNomenclatureSubGroup { get; set; }
        public string Barcode { get; set; }
        public int Count { get; set; }
        public string ExchangeRate { get; set; }
        public string UnitStorage { get; set; }
        public Nullable<int> WarrantyPeriod { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string Title { get; set; }
        public string VendorCode { get; set; }

        public string PeriodString
        {
            get
            {
                if (WarrantyPeriod == null || WarrantyPeriod == 0) return "Нет";
                else return WarrantyPeriod.ToString();
            }
        }
    }
}

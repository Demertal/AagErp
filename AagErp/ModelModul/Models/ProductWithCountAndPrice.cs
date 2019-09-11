using System.Collections.Generic;

namespace ModelModul.Models
{
    public class ProductWithCountAndPrice
    {
        public ProductWithCountAndPrice()
        {
        }

        public long Id { get; set; }

        public string Title { get; set; }

        public string VendorCode { get; set; }

        public string Barcode { get; set; }

        public string Description { get; set; }

        public int WarrantyPeriodId { get; set; }

        public int CategoryId { get; set; }

        public int PriceGroupId { get; set; }

        public int UnitStorageId { get; set; }

        public bool KeepTrackSerialNumbers { get; set; }

        public decimal Count { get; set; }

        public decimal Price { get; set; }

        public virtual Category Category { get; set; }

        public virtual PriceGroup PriceGroup { get; set; }

        public virtual UnitStorage UnitStorage { get; set; }

        public virtual WarrantyPeriod WarrantyPeriod { get; set; }


        public virtual ICollection<InvoiceInfo> InvoiceInfosCollection { get; set; }

        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfosCollection { get; set; }

        public virtual ICollection<PriceProduct> PriceProductsCollection { get; set; }

        public virtual ICollection<PropertyProduct> PropertyProductsCollection { get; set; }

        public virtual ICollection<SerialNumber> SerialNumbersCollection { get; set; }

        public static implicit operator ProductWithCountAndPrice(Product p)
        {
            return new ProductWithCountAndPrice
            {
                Barcode = p.Barcode,
                Count = p.Count,
                Category = p.Category,
                Id = p.Id,
                Price = p.Price,
                Title = p.Title,
                PriceGroupId = p.IdPriceGroup,
                Description = p.Description,
                PriceGroup = p.PriceGroup,
                KeepTrackSerialNumbers = p.KeepTrackSerialNumbers,
                UnitStorage = p.UnitStorage,
                UnitStorageId = p.IdUnitStorage,
                CategoryId = p.IdCategory,
                WarrantyPeriodId = p.IdWarrantyPeriod,
                VendorCode = p.VendorCode,
                PriceProductsCollection = p.PriceProductsCollection,
                InvoiceInfosCollection = p.InvoiceInfosCollection,
                MovementGoodsInfosCollection = p.MovementGoodsInfosCollection,
                PropertyProductsCollection = p.PropertyProductsCollection,
                SerialNumbersCollection = p.SerialNumbersCollection,
                WarrantyPeriod = p.WarrantyPeriod
            };
        }

        public static explicit operator Product(ProductWithCountAndPrice p)
        {
            return new Product
            {
                Barcode = p.Barcode,
                Count = p.Count,
                Category = p.Category,
                Id = p.Id,
                Price = p.Price,
                Title = p.Title,
                IdPriceGroup = p.PriceGroupId,
                Description = p.Description,
                PriceGroup = p.PriceGroup,
                KeepTrackSerialNumbers = p.KeepTrackSerialNumbers,
                UnitStorage = p.UnitStorage,
                IdUnitStorage = p.UnitStorageId,
                IdCategory = p.CategoryId,
                IdWarrantyPeriod = p.WarrantyPeriodId,
                VendorCode = p.VendorCode,
                PriceProductsCollection = p.PriceProductsCollection,
                InvoiceInfosCollection = p.InvoiceInfosCollection,
                MovementGoodsInfosCollection = p.MovementGoodsInfosCollection,
                PropertyProductsCollection = p.PropertyProductsCollection,
                SerialNumbersCollection = p.SerialNumbersCollection,
                WarrantyPeriod = p.WarrantyPeriod
            };
        }
    }
}

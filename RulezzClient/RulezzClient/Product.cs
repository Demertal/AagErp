//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RulezzClient
{
    using System.Collections.Generic;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.PropertyProduct = new HashSet<PropertyProduct>();
            this.PurchaseInfo = new HashSet<PurchaseInfo>();
            this.RevaluationProduct = new HashSet<RevaluationProduct>();
            this.SalesInfo = new HashSet<SalesInfo>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string VendorCode { get; set; }
        public string Barcode { get; set; }
        public int Count { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public int IdNomenclatureSubGroup { get; set; }
        public int IdUnitStorage { get; set; }
        public int IdExchangeRate { get; set; }
        public int IdWarrantyPeriod { get; set; }
    
        public virtual ExchangeRate ExchangeRate { get; set; }
        public virtual NomenclatureSubGroup NomenclatureSubGroup { get; set; }
        public virtual UnitStorage UnitStorage { get; set; }
        public virtual WarrantyPeriod WarrantyPeriod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyProduct> PropertyProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseInfo> PurchaseInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RevaluationProduct> RevaluationProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesInfo> SalesInfo { get; set; }
    }
}

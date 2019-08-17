namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public products()
        {
            invoiceInfos = new HashSet<invoiceInfos>();
            movementGoodsInfos = new HashSet<movementGoodsInfos>();
            priceProducts = new HashSet<priceProducts>();
            propertyProducts = new HashSet<propertyProducts>();
            serialNumbers = new HashSet<serialNumbers>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(120)]
        public string title { get; set; }

        [StringLength(20)]
        public string vendorCode { get; set; }

        [StringLength(13)]
        public string barcode { get; set; }

        public int idWarrantyPeriod { get; set; }

        public int idCategory { get; set; }

        public int? idPriceGroup { get; set; }

        public int idUnitStorage { get; set; }

        public bool keepTrackSerialNumbers { get; set; }

        public virtual categories categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoiceInfos> invoiceInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movementGoodsInfos> movementGoodsInfos { get; set; }

        public virtual priceGroups priceGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<priceProducts> priceProducts { get; set; }

        public virtual unitStorages unitStorages { get; set; }

        public virtual warrantyPeriods warrantyPeriods { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<propertyProducts> propertyProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serialNumbers> serialNumbers { get; set; }
    }
}

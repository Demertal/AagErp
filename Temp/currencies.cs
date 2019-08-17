namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class currencies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public currencies()
        {
            movementGoods = new HashSet<movementGoods>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(10)]
        public string title { get; set; }

        [Column(TypeName = "money")]
        public decimal cost { get; set; }

        public bool isDefault { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movementGoods> movementGoods { get; set; }
    }
}

namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class counterparties
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public counterparties()
        {
            moneyTransfers = new HashSet<moneyTransfers>();
            movementGoods = new HashSet<movementGoods>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(40)]
        public string title { get; set; }

        [StringLength(40)]
        public string contactPerson { get; set; }

        [StringLength(50)]
        public string contactPhone { get; set; }

        [StringLength(40)]
        public string props { get; set; }

        [StringLength(40)]
        public string address { get; set; }

        public int whoIsIt { get; set; }

        public int? idPaymentType { get; set; }

        public virtual paymentTypes paymentTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<moneyTransfers> moneyTransfers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movementGoods> movementGoods { get; set; }
    }
}

namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serialNumbers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public serialNumbers()
        {
            serialNumberLogs = new HashSet<serialNumberLogs>();
            warranties = new HashSet<warranties>();
            warranties1 = new HashSet<warranties>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(20)]
        public string value { get; set; }

        public long idProduct { get; set; }

        public DateTime dateCreated { get; set; }

        public virtual products products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serialNumberLogs> serialNumberLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<warranties> warranties { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<warranties> warranties1 { get; set; }
    }
}

namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class propertyNames
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public propertyNames()
        {
            propertyProducts = new HashSet<propertyProducts>();
            propertyValues = new HashSet<propertyValues>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string title { get; set; }

        public int? idCategory { get; set; }

        public virtual categories categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<propertyProducts> propertyProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<propertyValues> propertyValues { get; set; }
    }
}

namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class propertyValues
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public propertyValues()
        {
            propertyProducts = new HashSet<propertyProducts>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string value { get; set; }

        public int idPropertyName { get; set; }

        public virtual propertyNames propertyNames { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<propertyProducts> propertyProducts { get; set; }
    }
}

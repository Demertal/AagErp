//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ModelModul
{
    public partial class PropertyValues
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyValues()
        {
            this.PropertyProducts = new HashSet<PropertyProducts>();
        }
    
        public int Id { get; set; }
        public string Value { get; set; }
        public int IdPropertyName { get; set; }
    
        public virtual PropertyNames PropertyNames { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyProducts> PropertyProducts { get; set; }
    }
}

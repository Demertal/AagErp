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
    using System;
    using System.Collections.Generic;
    
    public partial class NomenclatureSubGroup : IEquatable<NomenclatureSubGroup>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NomenclatureSubGroup()
        {
            this.Product = new HashSet<Product>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdNomenclatureGroup { get; set; }
        public int IdPriceGroup { get; set; }
    
        public virtual NomenclatureGroup NomenclatureGroup { get; set; }
        public virtual PriceGroup PriceGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }

        public bool Equals(NomenclatureSubGroup obj)
        {
            if (this.Id == obj.Id && this.Title == obj.Title && this.IdNomenclatureGroup == obj.IdNomenclatureGroup && this.IdPriceGroup == obj.IdPriceGroup) return true;
            else return false;
        }
    }
}

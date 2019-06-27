//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    
    public partial class Counterparties
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Counterparties()
        {
            this.PurchaseReports = new HashSet<PurchaseReports>();
            this.SalesReports = new HashSet<SalesReports>();
            this.SerialNumbers = new HashSet<SerialNumbers>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Props { get; set; }
        public string Address { get; set; }
        public bool WhoIsIt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseReports> PurchaseReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesReports> SalesReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SerialNumbers> SerialNumbers { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ModelModul
{
    public partial class PurchaseReports
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseReports()
        {
            PurchaseInfos = new HashSet<PurchaseInfos>();
        }
    
        public int Id { get; set; }
        public DateTime? DataOrder { get; set; }
        public decimal Course { get; set; }
        public string TextInfo { get; set; }
        public int IdStore { get; set; }
        public int IdCounterparty { get; set; }
    
        public virtual Counterparties Counterparties { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseInfos> PurchaseInfos { get; set; }
        public virtual Stores Stores { get; set; }
    }
}

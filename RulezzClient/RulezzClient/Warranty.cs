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

    public partial class Warranty
    {
        public int Id { get; set; }
        public string Malfunction { get; set; }
        public System.DateTime DateReceipt { get; set; }
        public System.DateTime DateDeparture { get; set; }
        public System.DateTime DateIssue { get; set; }
        public string Info { get; set; }
        public int IdSupplier { get; set; }
        public Nullable<int> IdSerialNumber { get; set; }
    
        public virtual SerialNumber SerialNumber { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}

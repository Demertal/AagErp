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
    
    public partial class SalesInfo
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal SellingPrice { get; set; }
        public int IdProduct { get; set; }
        public int IdSalesReport { get; set; }
        public Nullable<int> IdSerialNumber { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SalesReport SalesReport { get; set; }
        public virtual SerialNumber SerialNumber { get; set; }
    }
}

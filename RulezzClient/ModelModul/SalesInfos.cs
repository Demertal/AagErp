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
    public partial class SalesInfos
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal SellingPrice { get; set; }
        public int IdProduct { get; set; }
        public int IdSalesReport { get; set; }
        public int? IdSerialNumber { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual SalesReports SalesReports { get; set; }
        public virtual SerialNumbers SerialNumbers { get; set; }
    }
}

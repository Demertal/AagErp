namespace ModelModul.Models
{
    public class ProfitStatement
    {
        public long ProductId { get; set; }
        public decimal Count { get; set; }
        public decimal Summa { get; set; }
        public virtual Product Product { get; set; }
    }
}

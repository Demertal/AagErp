namespace ModelModul.Models
{
    public class CountsProduct
    {
        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Count { get; set; }
    }
}

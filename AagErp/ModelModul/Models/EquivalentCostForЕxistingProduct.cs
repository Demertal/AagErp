﻿namespace ModelModul.Models
{
    public class EquivalentCostForЕxistingProduct
    {
        public decimal Count { get; set; }

        public decimal EquivalentCost { get; set; }

        public int EquivalentCurrencyId { get; set; }

        public virtual Currency EquivalentCurrency { get; set; }
    }
}

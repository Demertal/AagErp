namespace ModelModul.Models
{
    public class EquivalentCostForЕxistingProduct : ModelBase
    {
        private double _count;
        public double Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        private decimal _equivalentCost;
        public decimal EquivalentCost
        {
            get => _equivalentCost;
            set
            {
                _equivalentCost = value;
                OnPropertyChanged("EquivalentCost");
            }
        }

        private int _equivalentCurrencyId;
        public int EquivalentCurrencyId
        {
            get => _equivalentCurrencyId;
            set
            {
                _equivalentCurrencyId = value;
                OnPropertyChanged("EquivalentCurrencyId");
            }
        }

        private Currency _equivalentCurrency;
        public virtual Currency EquivalentCurrency
        {
            get => _equivalentCurrency;
            set
            {
                _equivalentCurrency = value;
                OnPropertyChanged("EquivalentCurrency");
            }
        }
    }
}

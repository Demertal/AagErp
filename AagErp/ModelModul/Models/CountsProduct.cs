namespace ModelModul.Models
{
    public class CountsProduct : ModelBase
    {
        private int _storeId;
        public int StoreId
        {
            get => _storeId;
            set
            {
                _storeId = value;
                OnPropertyChanged("StoreId");
            }
        }

        private Store _store;
        public virtual Store Store
        {
            get => _store;
            set
            {
                _store = value;
                OnPropertyChanged("Store");
            }
        }

        private decimal _count;
        public decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }
    }
}

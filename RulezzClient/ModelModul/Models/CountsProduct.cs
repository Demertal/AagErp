
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
                _store.PropertyChanged += _store_PropertyChanged;
                OnPropertyChanged("Store");
            }
        }

        private void _store_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Store");
        }

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
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient
{
    class StoreListVm : BindableBase
    {
        private readonly ObservableCollection<StoreVm> _storeList = new ObservableCollection<StoreVm>();
        public ReadOnlyObservableCollection<StoreVm> Stores;

        public StoreListVm()
        {
            Stores = new ReadOnlyObservableCollection<StoreVm>(_storeList);
        }

        public async Task<List<StoreVm>> GetListStore(string connectionString)
        {
            List<Store> tempM =  await Task.Run(() => Store.AsyncLoad(connectionString));
            List<StoreVm> tempVm = new List<StoreVm>();
            if (tempM == null){ _storeList.Clear(); return tempVm;}
            tempVm = new List<StoreVm>(tempM.Select(t => new StoreVm(t)));
            for (int i = 0; i < _storeList.Count; i++)
            {
                if (tempVm.Contains(_storeList[i])) tempVm.Remove(_storeList[i]);
                else
                {
                    _storeList.Remove(_storeList[i]);
                    i--;
                }
                if(_storeList.Count == 0) break;
            }

            foreach (var store in tempVm)
            {
                if (!_storeList.Contains(store)) _storeList.Add(store);
            }

            return tempVm;
        }
    }

    class StoreVm : BindableBase
    {
        private readonly Store _store;

        public StoreVm(Store store)
        {
            _store = store;
        }

        public string Title
        {
            get => _store.Title;
            set
            {
                _store.Title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _store.Id;
            set
            {
                _store.Id = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(StoreVm other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title;
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class StoreListVm : BindableBase
    {
        private readonly ObservableCollection<Store> _storeList = new ObservableCollection<Store>();
        public ReadOnlyObservableCollection<Store> Stores;

        public StoreListVm()
        {
            Stores = new ReadOnlyObservableCollection<Store>(_storeList);
        }

        public async Task<List<Store>> GetListStore()
        {
            List<Store> tempM =  await Task.Run(() => Store.AsyncLoad(Properties.Settings.Default.СconnectionString));
            if (tempM == null){ _storeList.Clear(); return null; }
            for (int i = 0; i < _storeList.Count; i++)
            {
                if (tempM.Contains(_storeList[i])) tempM.Remove(_storeList[i]);
                else
                {
                    _storeList.Remove(_storeList[i]);
                    i--;
                }
                if(_storeList.Count == 0) break;
            }

            foreach (var store in tempM)
            {
                if (!_storeList.Contains(store)) _storeList.Add(store);
            }

            return tempM;
        }
    }
}

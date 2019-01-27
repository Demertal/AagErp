using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class StoreListVm : BindableBase
    {
        private readonly ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        public ReadOnlyObservableCollection<Store> Stores;

        public StoreListVm()
        {
            Stores = new ReadOnlyObservableCollection<Store>(_stores);
        }

        public async Task<List<Store>> Load()
        {
            List<Store> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.Store.SqlQuery("Select * From Store").ToList();
                }
            });

            _stores.Clear();
            foreach (var store in temp)
            {
                _stores.Add(store);
            }
            return temp;
        }
    }
}

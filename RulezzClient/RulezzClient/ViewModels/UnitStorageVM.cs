using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class UnitStorageListVm : BindableBase
    {
        private readonly ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages;

        public UnitStorageListVm()
        {
            UnitStorages = new ReadOnlyObservableCollection<UnitStorage>(_unitStorages);
        }

        public async Task<List<UnitStorage>> Load()
        {
            List<UnitStorage> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.UnitStorage.SqlQuery("Select * From UnitStorage").ToList();
                }
            });

            _unitStorages.Clear();
            foreach (var unitStorage in temp)
            {
                _unitStorages.Add(unitStorage);
            }
            return temp;
        }
    }
}

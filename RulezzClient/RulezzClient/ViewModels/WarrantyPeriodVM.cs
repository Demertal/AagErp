using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class WarrantyPeriodListVm : BindableBase
    {
        private readonly ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods;

        public WarrantyPeriodListVm()
        {
            WarrantyPeriods = new ReadOnlyObservableCollection<WarrantyPeriod>(_warrantyPeriods);
        }

        public async Task<List<WarrantyPeriod>> Load()
        {
            List<WarrantyPeriod> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.WarrantyPeriod.SqlQuery("Select * From WarrantyPeriod").ToList();
                }
            });

            _warrantyPeriods.Clear();
            foreach (var warrantyPeriod in temp)
            {
                _warrantyPeriods.Add(warrantyPeriod);
            }
            return temp;
        }
        
    }
}

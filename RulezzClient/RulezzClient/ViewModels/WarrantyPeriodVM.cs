//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class WarrantyPeriodListVm : BindableBase
//    {
//        private readonly ObservableCollection<WarrantyPeriods> _warrantyPeriods = new ObservableCollection<WarrantyPeriods>();
//        public ReadOnlyObservableCollection<WarrantyPeriods> WarrantyPeriods;

//        public WarrantyPeriodListVm()
//        {
//            WarrantyPeriods = new ReadOnlyObservableCollection<WarrantyPeriods>(_warrantyPeriods);
//        }

//        public async Task<List<WarrantyPeriods>> Load()
//        {
//            List<WarrantyPeriods> temp = await Task.Run(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    return db.WarrantyPeriods.SqlQuery("Select * From WarrantyPeriods").ToList();
//                }
//            });

//            _warrantyPeriods.Clear();
//            foreach (var warrantyPeriod in temp)
//            {
//                _warrantyPeriods.Add(warrantyPeriod);
//            }
//            return temp;
//        }
        
//    }
//}

//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class UnitStorageListVm : BindableBase
//    {
//        private readonly ObservableCollection<UnitStorages> _unitStorages = new ObservableCollection<UnitStorages>();
//        public ReadOnlyObservableCollection<UnitStorages> UnitStorages;

//        public UnitStorageListVm()
//        {
//            UnitStorages = new ReadOnlyObservableCollection<UnitStorages>(_unitStorages);
//        }

//        public async Task<List<UnitStorages>> Load()
//        {
//            List<UnitStorages> temp = await Task.Run(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    return db.UnitStorages.SqlQuery("Select * From UnitStorages").ToList();
//                }
//            });

//            _unitStorages.Clear();
//            foreach (var unitStorage in temp)
//            {
//                _unitStorages.Add(unitStorage);
//            }
//            return temp;
//        }
//    }
//}

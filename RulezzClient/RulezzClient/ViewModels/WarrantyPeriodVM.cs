using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class WarrantyPeriodListVm : BindableBase
    {
        private readonly ObservableCollection<WarrantyPeriod> _warrantyPeriodList = new ObservableCollection<WarrantyPeriod>();
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods;

        public WarrantyPeriodListVm()
        {
            WarrantyPeriods = new ReadOnlyObservableCollection<WarrantyPeriod>(_warrantyPeriodList);
        }

        public async Task<List<WarrantyPeriod>> GetListWarrantyPeriod()
        {
            List<WarrantyPeriod> tempM = await Task.Run(() => WarrantyPeriod.AsyncLoad(Properties.Settings.Default.СconnectionString));
            if (tempM == null) { _warrantyPeriodList.Clear(); return null; }
            for (int i = 0; i < _warrantyPeriodList.Count; i++)
            {
                if (tempM.Contains(_warrantyPeriodList[i])) tempM.Remove(_warrantyPeriodList[i]);
                else
                {
                    _warrantyPeriodList.Remove(_warrantyPeriodList[i]);
                    i--;
                }
                if (_warrantyPeriodList.Count == 0) break;
            }

            foreach (var warrantyPeriod in tempM)
            {
                if (!_warrantyPeriodList.Contains(warrantyPeriod)) _warrantyPeriodList.Add(warrantyPeriod);
            }

            return tempM;
        }
    }
}

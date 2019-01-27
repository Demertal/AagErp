using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class UnitStorageListVm : BindableBase
    {
        private readonly ObservableCollection<UnitStorage> _unitStorageList = new ObservableCollection<UnitStorage>();
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages;

        public UnitStorageListVm()
        {
            UnitStorages = new ReadOnlyObservableCollection<UnitStorage>(_unitStorageList);
        }

        public async Task<List<UnitStorage>> GetListUnitStorage()
        {
            List<UnitStorage> tempM = await Task.Run(() => UnitStorage.AsyncLoad(Properties.Settings.Default.СconnectionString));
            if (tempM == null) { _unitStorageList.Clear(); return null; }
            for (int i = 0; i < _unitStorageList.Count; i++)
            {
                if (tempM.Contains(_unitStorageList[i])) tempM.Remove(_unitStorageList[i]);
                else
                {
                    _unitStorageList.Remove(_unitStorageList[i]);
                    i--;
                }
                if (_unitStorageList.Count == 0) break;
            }

            foreach (var unitStorage in tempM)
            {
                if (!_unitStorageList.Contains(unitStorage)) _unitStorageList.Add(unitStorage);
            }

            return tempM;
        }
    }
}

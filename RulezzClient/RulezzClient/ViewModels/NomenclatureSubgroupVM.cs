using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class NomenclatureSubgroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureSubgroup> _nomenclatureSubgroupList = new ObservableCollection<NomenclatureSubgroup>();
        public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups;

        public NomenclatureSubgroupListVm()
        {
            NomenclatureSubgroups = new ReadOnlyObservableCollection<NomenclatureSubgroup>(_nomenclatureSubgroupList);
        }

        public async Task<List<NomenclatureSubgroup>> GetListNomenclatureSubgroup(int idNomenclatureGroup)
        {
            List<NomenclatureSubgroup> tempM =
                await Task.Run(() => NomenclatureSubgroup.AsyncLoad(Properties.Settings.Default.СconnectionString, idNomenclatureGroup));
            List<NomenclatureSubgroup> tempVm = new List<NomenclatureSubgroup>();
            if (tempM == null){ _nomenclatureSubgroupList.Clear(); return tempVm;}
            tempVm = new List<NomenclatureSubgroup>(tempM.Select(t => new NomenclatureSubgroup(t)));
            for (int i = 0; i < _nomenclatureSubgroupList.Count; i++)
            {
                if (tempVm.Contains(_nomenclatureSubgroupList[i])) tempVm.Remove(_nomenclatureSubgroupList[i]);
                else
                {
                    _nomenclatureSubgroupList.Remove(_nomenclatureSubgroupList[i]);
                    i--;
                }
                if (_nomenclatureSubgroupList.Count == 0) break;
            }

            foreach (var store in tempVm)
            {
                if (!_nomenclatureSubgroupList.Contains(store)) _nomenclatureSubgroupList.Add(store);
            }
            return tempVm;
        }
    }
}

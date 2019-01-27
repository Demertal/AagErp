using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class NomenclatureGroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureGroup> _nomenclatureGroupList = new ObservableCollection<NomenclatureGroup>();
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups;

        public NomenclatureGroupListVm()
        {
            NomenclatureGroups = new ReadOnlyObservableCollection<NomenclatureGroup>(_nomenclatureGroupList);
        }

        public async Task<List<NomenclatureGroup>> GetListNomenclatureGroup(int idStore)
        {
            List<NomenclatureGroup> tempM =
                await Task.Run(() => NomenclatureGroup.AsyncLoad(Properties.Settings.Default.СconnectionString, idStore));
            if (tempM == null){ _nomenclatureGroupList.Clear(); return null;}
            for (int i = 0; i < _nomenclatureGroupList.Count; i++)
            {
                if (tempM.Contains(_nomenclatureGroupList[i])) tempM.Remove(_nomenclatureGroupList[i]);
                else
                {
                    _nomenclatureGroupList.Remove(_nomenclatureGroupList[i]);
                    i--;
                }
                if (_nomenclatureGroupList.Count == 0) break;
            }

            foreach (var store in tempM)
            {
                if (!_nomenclatureGroupList.Contains(store)) _nomenclatureGroupList.Add(store);
            }
            return tempM;
        }
    }
}

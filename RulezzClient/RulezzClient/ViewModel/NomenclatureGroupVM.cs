using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient
{
    class NomenclatureGroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureGroupVm> _nomenclatureGroupList = new ObservableCollection<NomenclatureGroupVm>();
        public ReadOnlyObservableCollection<NomenclatureGroupVm> NomenclatureGroups;

        public NomenclatureGroupListVm()
        {
            NomenclatureGroups = new ReadOnlyObservableCollection<NomenclatureGroupVm>(_nomenclatureGroupList);
        }

        public async Task<List<NomenclatureGroupVm>> GetListNomenclatureGroup(string connectionString, int idStore)
        {
            List<NomenclatureGroup> tempM =
                await Task.Run(() => NomenclatureGroup.AsyncLoad(connectionString, idStore));
            List<NomenclatureGroupVm> tempVm = new List<NomenclatureGroupVm>();
            if (tempM == null){ _nomenclatureGroupList.Clear(); return tempVm;}
            tempVm = new List<NomenclatureGroupVm>(tempM.Select(t => new NomenclatureGroupVm(t)));
            for (int i = 0; i < _nomenclatureGroupList.Count; i++)
            {
                if (tempVm.Contains(_nomenclatureGroupList[i])) tempVm.Remove(_nomenclatureGroupList[i]);
                else
                {
                    _nomenclatureGroupList.Remove(_nomenclatureGroupList[i]);
                    i--;
                }
                if (_nomenclatureGroupList.Count == 0) break;
            }

            foreach (var store in tempVm)
            {
                if (!_nomenclatureGroupList.Contains(store)) _nomenclatureGroupList.Add(store);
            }
            return tempVm;
        }
    }

    class NomenclatureGroupVm: BindableBase
    {
        private readonly NomenclatureGroup _nomenclatureGroup;

        public NomenclatureGroupVm(NomenclatureGroup nomenclatureGroup)
        {
            _nomenclatureGroup = nomenclatureGroup;
        }

        public string Title
        {
            get => _nomenclatureGroup.Title;
            set
            {
                _nomenclatureGroup.Title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _nomenclatureGroup.Id;
            set
            {
                _nomenclatureGroup.Id = value;
                RaisePropertyChanged();
            }
        }

        public int IdStore
        {
            get => _nomenclatureGroup.IdStore;
            set
            {
                _nomenclatureGroup.IdStore = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(NomenclatureGroupVm other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title && IdStore == other.IdStore;
        }
    }
}

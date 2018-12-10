using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient
{
    class NomenclatureSubgroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureSubgroupVm> _nomenclatureSubgroupList = new ObservableCollection<NomenclatureSubgroupVm>();
        public ReadOnlyObservableCollection<NomenclatureSubgroupVm> NomenclatureSubgroups;

        public NomenclatureSubgroupListVm()
        {
            NomenclatureSubgroups = new ReadOnlyObservableCollection<NomenclatureSubgroupVm>(_nomenclatureSubgroupList);
        }

        public async Task<List<NomenclatureSubgroupVm>> GetListNomenclatureSubgroup(int idNomenclatureGroup)
        {
            List<NomenclatureSubgroup> tempM =
                await Task.Run(() => NomenclatureSubgroup.AsyncLoad(Properties.Settings.Default.СconnectionString, idNomenclatureGroup));
            List<NomenclatureSubgroupVm> tempVm = new List<NomenclatureSubgroupVm>();
            if (tempM == null){ _nomenclatureSubgroupList.Clear(); return tempVm;}
            tempVm = new List<NomenclatureSubgroupVm>(tempM.Select(t => new NomenclatureSubgroupVm(t)));
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

    class NomenclatureSubgroupVm : BindableBase
    {
        private readonly NomenclatureSubgroup _nomenclatureSubgroup;

        public NomenclatureSubgroupVm(NomenclatureSubgroup nomenclatureSubgroup)
        {
            _nomenclatureSubgroup = nomenclatureSubgroup;
        }

        public string Title
        {
            get => _nomenclatureSubgroup.Title;
            set
            {
                _nomenclatureSubgroup.Title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _nomenclatureSubgroup.Id;
            set
            {
                _nomenclatureSubgroup.Id = value;
                RaisePropertyChanged();
            }
        }

        public int IdPriceGroup
        {
            get => _nomenclatureSubgroup.IdPriceGroup;
            set
            {
                _nomenclatureSubgroup.IdPriceGroup = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(NomenclatureSubgroupVm other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title && IdPriceGroup == other.IdPriceGroup;
        }
    }
}

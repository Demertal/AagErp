using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class NomenclatureGroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureGroup> _nomenclatureGroups = new ObservableCollection<NomenclatureGroup>();
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups;

        public NomenclatureGroupListVm()
        {
            NomenclatureGroups = new ReadOnlyObservableCollection<NomenclatureGroup>(_nomenclatureGroups);
        }

        public async Task<List<NomenclatureGroup>> Load(int idStore)
        {
            List<NomenclatureGroup> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.NomenclatureGroup.SqlQuery("Select * From NomenclatureGroup WHERE NomenclatureGroup.IdStore = @idStore",
                        new SqlParameter("@idStore", idStore)).ToList();
                }
            });

            _nomenclatureGroups.Clear();
            foreach (var nomenclatureGroup in temp)
            {
                _nomenclatureGroups.Add(nomenclatureGroup);
            }
            return temp;
        }
    }
}

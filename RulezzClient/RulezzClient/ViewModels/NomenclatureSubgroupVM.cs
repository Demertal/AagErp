using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class NomenclatureSubgroupListVm : BindableBase
    {
        private readonly ObservableCollection<NomenclatureSubGroup> _nomenclatureSubGroups = new ObservableCollection<NomenclatureSubGroup>();
        public ReadOnlyObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups;

        public NomenclatureSubgroupListVm()
        {
            NomenclatureSubGroups = new ReadOnlyObservableCollection<NomenclatureSubGroup>(_nomenclatureSubGroups);
        }

        public async Task<List<NomenclatureSubGroup>> Load(int idNomenclatureGroup)
        {
            List<NomenclatureSubGroup> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.NomenclatureSubGroup.SqlQuery("Select * From NomenclatureSubGroup WHERE NomenclatureSubGroup.IdNomenclatureGroup = @idNomenclatureGroup",
                        new SqlParameter("@idNomenclatureGroup", idNomenclatureGroup)).ToList();
                }
            });

            _nomenclatureSubGroups.Clear();
            foreach (var nomenclatureSubGroup in temp)
            {
                _nomenclatureSubGroups.Add(nomenclatureSubGroup);
            }
            return temp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace RulezzClient
{
   public class NomenclatureGroup : IObject
    {
        public string Title { get; set; }

        public int Id { get; set; }

        [Column(Name = "id_store")]
        public int IdStore { get; set; }

        private static List<NomenclatureGroup> Load(string connectionString, int idStore)
        {
            List<NomenclatureGroup> listNomenclatureGroup = new List<NomenclatureGroup>();
            try
            {
                NomenclatureGroupDataContext dc = new NomenclatureGroupDataContext(connectionString);
                var nomenclatureGroup = dc.Load(idStore);
                foreach (var ng in nomenclatureGroup)
                {
                    listNomenclatureGroup.Add(ng);
                }
                return listNomenclatureGroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<NomenclatureGroup>> AsyncLoad(string connectionString, int idStore)
        {
            return await Task.Run(() => Load(connectionString, idStore));
        }
    }

    public class NomenclatureGroupDataContext : DataContext
    {
        public NomenclatureGroupDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение номенклатурной группы по id магазина
        [Function(Name = "FunViewNomenclatureGroup", IsComposable = true)]
        public IQueryable<NomenclatureGroup> Load([Parameter(Name = "id_store", DbType = "int")] int idStore)
        {
            return CreateMethodCallQuery<NomenclatureGroup>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idStore);
        }
    }
}

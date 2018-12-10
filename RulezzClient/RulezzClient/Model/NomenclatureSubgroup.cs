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
    //Класс хранящий таблицу для получения наименования и id ценовой группы номенклатурной группы по id номенклатуры
    public class NomenclatureSubgroup : IObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Column(Name = "id_price_group")]
        public int IdPriceGroup { get; set; }

        private static List<NomenclatureSubgroup> Load(string connectionString, int idNomenclatureGroup)
        {
            List<NomenclatureSubgroup> listNomenclatureSubgroup = new List<NomenclatureSubgroup>();
            try
            {
                NomenclatureSubgroupDataContext dc = new NomenclatureSubgroupDataContext(connectionString);
                var nomenclatureSubgroup = dc.Load(idNomenclatureGroup);
                foreach (var ng in nomenclatureSubgroup)
                {
                    listNomenclatureSubgroup.Add(ng);
                }
                return listNomenclatureSubgroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<NomenclatureSubgroup>> AsyncLoad(string connectionString, int idNomenclatureGroup)
        {
            return await Task.Run(() => Load(connectionString, idNomenclatureGroup));
        }
    }

    public class NomenclatureSubgroupDataContext : DataContext
    {
        public NomenclatureSubgroupDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение наименования и ценовой группы номенклатурной подгруппы по id номенклатурной группы
        [Function(Name = "FunViewNomenclatureSubgroup", IsComposable = true)]
        public IQueryable<NomenclatureSubgroup> Load([Parameter(Name = "id_nomenclature_group", DbType = "int")] int idNomenclatureGroup)
        {
            return CreateMethodCallQuery<NomenclatureSubgroup>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomenclatureGroup);
        }
    }
}
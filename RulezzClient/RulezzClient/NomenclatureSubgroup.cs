using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования и id ценовой группы номенклатурной группы по id номенклатуры
    public class NomenclatureSubgroup : IObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Column(Name = "id_price_group")]
        public int IdPriceGroup { get; set; }
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
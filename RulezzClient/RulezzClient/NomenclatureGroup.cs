using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования номенклатуры по id магазина
    public class NomenclatureGroup : IObject
    {
        public string Title { get; set; }

        public int Id { get; set; }
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

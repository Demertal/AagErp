using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования номенклатуры по id магазина
    public class NomenclatureGroupResult
    {
        [Column(Name = "title")]
        public string Title { get; set; }
    }

    public class NomenclatureGroupDataContext : DataContext
    {
        public NomenclatureGroupDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение id номенклатуры группы по наименованию и id магазина
        [Function(Name = "FindIdNomenclatureGroup")]
        [return: Parameter(DbType = "Int")]
        public int FindNomenclatureGroupId(
            [Parameter(Name = "title", DbType = "nvarchar(20)")] string title,
            [Parameter(Name = "id_store", DbType = "int")] int idStore,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idStore, id);
            if (result == null) return -1;
            id = (int) result.GetParameterValue(2);
            return (int) result.ReturnValue;
        }

        //Получение наименования номенклатурной группы по id магазина
        [Function(Name = "FunViewNomenclatureGroup", IsComposable = true)]
        public IQueryable<NomenclatureGroupResult> GetNomenclatureGroup([Parameter(Name = "id_store", DbType = "int")] int idStore)
        {
            return CreateMethodCallQuery<NomenclatureGroupResult>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idStore);
        }
    }
}

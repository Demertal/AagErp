using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования номенклатуры по id магазина
    public class NomenclatureResult
    {
        [Column(Name = "title")]
        public string Title { get; set; }
    }

    public class NomenclatureDataContext : DataContext
    {
        public NomenclatureDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение id номенклатуры по наименованию и id магазина
        [Function(Name = "FindIdNomenclature")]
        [return: Parameter(DbType = "Int")]
        public int FindNomenclatureId(
            [Parameter(Name = "title", DbType = "nvarchar(20)")] string title,
            [Parameter(Name = "id_store", DbType = "int")] int idStore,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idStore, id);
            if (result == null) return -1;
            id = (int) result.GetParameterValue(2);
            return (int) result.ReturnValue;
        }

        //Получение наименования номенклатуры по id магазина
        [Function(Name = "FunViewNomenclature", IsComposable = true)]
        public IQueryable<NomenclatureResult> GetNomenclature([Parameter(Name = "id_store", DbType = "int")] int idStore)
        {
            return CreateMethodCallQuery<NomenclatureResult>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idStore);
        }
    }
}

using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования и id ценовой группы номенклатурной группы по id номенклатуры
    public class NomenclatureGroupeResult
    {
        [Column(Name = "title")]
        public string Title { get; set; }
        [Column(Name = "id_price_group")]
        public int IdPriceGroup { get; set; }
    }

    public class NomenclatureGroupeDataContext : DataContext
    {
        public NomenclatureGroupeDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение id номенклатурной группы по наименованию и id номенклатуры
        [Function(Name = "FindIdNomenclatureGroup")]
        [return: Parameter(DbType = "Int")]
        public int FindNomenclatureGroupId(
            [Parameter(Name = "title", DbType = "nvarchar(20)")] string title,
            [Parameter(Name = "id_nomenclature", DbType = "int")] int idNomenclature,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idNomenclature, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(2);
            return (int)result.ReturnValue;
        }

        //Получение наименования и ценовой группы номенклатурной группы по id номенклатупы
        [Function(Name = "FunViewNomenclatureGroup", IsComposable = true)]
        public IQueryable<NomenclatureGroupeResult> GetNomenclatureGroup([Parameter(Name = "id_nomenclature", DbType = "int")] int idNomenclature)
        {
            return CreateMethodCallQuery<NomenclatureGroupeResult>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomenclature);
        }
    }
}
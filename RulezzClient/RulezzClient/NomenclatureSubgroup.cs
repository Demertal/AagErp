using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace RulezzClient
{
    //Класс хранящий таблицу для получения наименования и id ценовой группы номенклатурной группы по id номенклатуры
    public class NomenclatureSubgroupResult
    {
        [Column(Name = "title")]
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

        //Получение id номенклатурной подгруппы по наименованию и id номенклатуры
        [Function(Name = "FindIdNomenclatureSubgroup")]
        [return: Parameter(DbType = "Int")]
        public int FindNomenclatureSubgroupId(
            [Parameter(Name = "title", DbType = "nvarchar(20)")] string title,
            [Parameter(Name = "id_nomenclature_group", DbType = "int")] int idNomenclatureGroup,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idNomenclatureGroup, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(2);
            return (int)result.ReturnValue;
        }

        //Получение наименования и ценовой группы номенклатурной подгруппы по id номенклатурной группы
        [Function(Name = "FunViewNomenclatureSubgroup", IsComposable = true)]
        public IQueryable<NomenclatureSubgroupResult> GetNomenclatureSubgroup([Parameter(Name = "id_nomenclature_group", DbType = "int")] int idNomenclatureGroup)
        {
            return CreateMethodCallQuery<NomenclatureSubgroupResult>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomenclatureGroup);
        }
    }
}
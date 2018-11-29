using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient
{
    public class ProductView
    {
        [Column(Name = "title")]
        public string Title { get; set; }

        [Column(Name = "vendor_code")]
        public string VendorCode { get; set; }

        [Column(Name = "count")]
        public int Count { get; set; }

        [Column(Name = "sales_price")]
        public decimal SalesPrice { get; set; }

        [Column(Name = "warranty")]
        public string Warranty { get; set; }
    }

    public class ProductDataContext : DataContext
    {
        public ProductDataContext(string connectionString)
            : base(connectionString)
        {

        }

        //Получение id номенклатуры по наименованию и id магазина
        //[Function(Name = "FindIdNomenclature")]
        //[return: Parameter(DbType = "Int")]
        //public int FindNomenclatureId(
        //    [Parameter(Name = "title", DbType = "nvarchar(20)")] string title,
        //    [Parameter(Name = "id_store", DbType = "int")] int idStore,
        //    [Parameter(Name = "id", DbType = "int")] ref int id)
        //{
        //    IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idStore, id);
        //    if (result == null) return -1;
        //    id = (int)result.GetParameterValue(2);
        //    return (int)result.ReturnValue;
        //}

        //Получение продукта по id номенклатурной группы
        [Function(Name = "FunViewProduct", IsComposable = true)]
        public IQueryable<ProductView> GetProduct([Parameter(Name = "id_nomenclature_group", DbType = "int")] int idNomenclatureGroup)
        {
            return CreateMethodCallQuery<ProductView>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomenclatureGroup);
        }
    }
}

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

        [Column(Name = "barcode")]
        public string Barcode { get; set; }

        [Column(Name = "count")]
        public int Count { get; set; }

        [Column(Name = "unit_storage")]
        public string UnitStorage { get; set; }

        [Column(Name = "sales_price")]
        public decimal SalesPrice { get; set; }

        [Column(Name = "purchase_price")]
        public decimal PurchasePrice { get; set; }

        [Column(Name = "warranty")]
        public string Warranty { get; set; }

        [Column(Name = "exchange_rates")]
        public string ExchangeRates { get; set; }
    }

    public class ProductDataContext : DataContext
    {
        public ProductDataContext(string connectionString)
            : base(connectionString)
        {

        }
        //Получение id продукта по id номенклатурной подгруппы
        [Function(Name = "FindIdProduct")]
        [return: Parameter(DbType = "Int")]
        public int FindProductId(
            [Parameter(Name = "barcode", DbType = "nvarchar(13)")] string barcode,
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")] int idNomSub,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), barcode, idNomSub, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(2);
            return (int)result.ReturnValue;
        }

        //Удаление товара
        [Function(Name = "DeleteProduct")]
        [return: Parameter(DbType = "Int")]
        public int Delete(
            [Parameter(Name = "barcode", DbType = "nvarchar(13)")] string barcode,
            [Parameter(Name = "id_nomeclature_subgroup", DbType = "int")] int idNomSub)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), barcode, idNomSub);
            if (result == null) return -1;
            return (int)result.ReturnValue;
        }

        //Получение продукта по id номенклатурной подгруппы
        [Function(Name = "FunViewProduct", IsComposable = true)]
        public IQueryable<ProductView> GetListProduct(
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")] int idNomSub,
            [Parameter(Name = "id_prod", DbType = "int")] int idProd)
        {
            return CreateMethodCallQuery<ProductView>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomSub, idProd);
        }
    }
}

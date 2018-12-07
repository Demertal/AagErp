using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient
{
    public class Product : IObject
    {
        public int Id { get; set; }

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

        //Удаление товара
        [Function(Name = "DeleteProduct")]
        [return: Parameter(DbType = "Int")]
        public int Delete(
            [Parameter(Name = "title", DbType = "nvarchar(120)")] string title,
            [Parameter(Name = "id_nomeclature_subgroup", DbType = "int")] int idNomSub)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idNomSub);
            if (result == null) return -1;
            return (int)result.ReturnValue;
        }

        //Получение продукта по id номенклатурной подгруппы
        [Function(Name = "FunViewProduct", IsComposable = true)]
        public IQueryable<Product> Load(
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")] int idNomSub)
        {
            return CreateMethodCallQuery<Product>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomSub);
        }
    }
}

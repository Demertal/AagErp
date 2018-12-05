using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RulezzClient.Annotations;

namespace RulezzClient
{
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _vendorCode;
        private string _barcode;
        private int _count;
        private string _unitStorage;
        private decimal _salesPrice;
        private decimal _purchasePrice;
        private string _warranty;
        private string _exchangeRates;

        [Column(Name = "id")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "title")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "vendor_code")]
        public string VendorCode
        {
            get => _vendorCode;
            set
            {
                _vendorCode = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "barcode")]
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "count")]
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "unit_storage")]
        public string UnitStorage
        {
            get => _unitStorage;
            set
            {
                _unitStorage = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "sales_price")]
        public decimal SalesPrice
        {
            get => _salesPrice;
            set
            {
                _salesPrice = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "purchase_price")]
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "warranty")]
        public string Warranty
        {
            get => _warranty;
            set
            {
                _warranty = value;
                OnPropertyChanged();
            }
        }

        [Column(Name = "exchange_rates")]
        public string ExchangeRates
        {
            get => _exchangeRates;
            set
            {
                _exchangeRates = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
            [Parameter(Name = "title", DbType = "nvarchar(120)")] string title,
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")] int idNomSub,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), title, idNomSub, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(2);
            return (int)result.ReturnValue;
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
        public IQueryable<Product> GetListProduct(
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")] int idNomSub,
            [Parameter(Name = "id_prod", DbType = "int")] int idProd)
        {
            return CreateMethodCallQuery<Product>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomSub, idProd);
        }
    }
}

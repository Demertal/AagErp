using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RulezzClient
{
    class ProductListVm : BindableBase
    {
        private readonly ObservableCollection<ProductVm> _productList =
            new ObservableCollection<ProductVm>();

        public ReadOnlyObservableCollection<ProductVm> Products;

        public ProductListVm()
        {
            Products = new ReadOnlyObservableCollection<ProductVm>(_productList);
        }

        public async Task<List<ProductVm>> GetListProduct(string connectionString, int idNomenclatureSubgroup)
        {
            List<Product> tempM =
                await Task.Run(() => Product.AsyncLoad(connectionString, idNomenclatureSubgroup));
            List<ProductVm> tempVm = new List<ProductVm>();
            if (tempM == null){ _productList.Clear(); return tempVm;}
            tempVm = new List<ProductVm>(tempM.Select(t => new ProductVm(t)));
            _productList.Clear();
            foreach (var t in tempVm)
            {
                _productList.Add(t);
            }
            return tempVm;
        }
    }

    class ProductVm : BindableBase
    {
        private readonly Product _product;

        public ProductVm(Product product)
        {
            _product = product;
        }

        public string Title
        {
            get => _product.Title;
            set
            {
                _product.Title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _product.Id;
            set
            {
                _product.Id = value;
                RaisePropertyChanged();
            }
        }

        public string VendorCode
        {
            get => _product.VendorCode;
            set
            {
                _product.VendorCode = value;
                RaisePropertyChanged();
            }
        }

        public string Barcode
        {
            get => _product.Barcode;
            set
            {
                _product.Barcode = value;
                RaisePropertyChanged();
            }
        }

        public int Count
        {
            get => _product.Count;
            set
            {
                _product.Count = value;
                RaisePropertyChanged();
            }
        }

        public string UnitStorage
        {
            get => _product.UnitStorage;
            set
            {
                _product.UnitStorage = value;
                RaisePropertyChanged();
            }
        }

        public decimal SalesPrice
        {
            get => _product.SalesPrice;
            set
            {
                _product.SalesPrice = value;
                RaisePropertyChanged();
            }
        }

        public decimal PurchasePrice
        {
            get => _product.PurchasePrice;
            set
            {
                _product.PurchasePrice = value;
                RaisePropertyChanged();
            }
        }

        public string Warranty
        {
            get => _product.Warranty;
            set
            {
                _product.Warranty = value;
                RaisePropertyChanged();
            }
        }

        public string ExchangeRates
        {
            get => _product.ExchangeRates;
            set
            {
                _product.ExchangeRates = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(ProductVm other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title && VendorCode == other.VendorCode &&
                   Barcode == other.Barcode && Count == other.Count && UnitStorage == other.UnitStorage &&
                   SalesPrice == other.SalesPrice && PurchasePrice == other.PurchasePrice &&
                   Warranty == other.Warranty && ExchangeRates == other.ExchangeRates;
        }
    }
}

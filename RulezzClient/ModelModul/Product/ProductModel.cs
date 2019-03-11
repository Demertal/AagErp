using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModelModul.Product
{
    public class ProductModel : INotifyPropertyChanged
    {
        #region Properties
        
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _vendorCode;
        public string VendorCode
        {
            get => _vendorCode;
            set
            {
                _vendorCode = value;
                OnPropertyChanged();
            }
        }

        private string _barcode;
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged();
            }
        }

        private decimal _salesPrice;
        public decimal SalesPrice
        {
            get => _salesPrice;
            set
            {
                _salesPrice = value;
                OnPropertyChanged();
            }
        }

        private int _idUnitStorage;
        public int IdUnitStorage
        {
            get => _idUnitStorage;
            set
            {
                _idUnitStorage = value;
                OnPropertyChanged();
            }
        }

        private int _idExchangeRate;
        public int IdExchangeRate
        {
            get => _idExchangeRate;
            set
            {
                _idExchangeRate = value;
                OnPropertyChanged();
            }
        }

        private int _idWarrantyPeriod;
        public int IdWarrantyPeriod
        {
            get => _idWarrantyPeriod;
            set
            {
                _idWarrantyPeriod = value;
                OnPropertyChanged();
            }
        }

        private int _idGroup;
        public int IdGroup
        {
            get => _idGroup;
            set
            {
                _idGroup = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion

        public Products ConvertToProducts()
        {
            return new Products
            {
                Id = Id,
                Title = Title,
                VendorCode = VendorCode,
                Barcode = Barcode,
                PurchasePrice = PurchasePrice,
                SalesPrice = SalesPrice,
                IdUnitStorage = IdUnitStorage,
                IdExchangeRate = IdExchangeRate,
                IdWarrantyPeriod = IdWarrantyPeriod,
                IdGroup = IdGroup
            };
        }
    }
}

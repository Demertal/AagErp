using System.Collections.ObjectModel;
using System.Linq;
using ModelModul.Product;
using Prism.Commands;

namespace ModelModul.PurchaseGoods
{
    public class PurchaseInfosViewModel: ProductViewModel
    {
        private PurchaseInfos _purchaseInfo;

        public PurchaseInfos PurchaseInfo
        {
            get => _purchaseInfo;
            set => SetProperty(ref _purchaseInfo, value);
        }

        public override int Id
        {
            get => _purchaseInfo.Id;
            set
            {
                _purchaseInfo.Id = value;
                RaisePropertyChanged();
            }
        }

        public new int Count
        {
            get => _purchaseInfo.Count;
            set
            {
                _purchaseInfo.Count = value;
                if (WarrantyPeriod != null && WarrantyPeriod.Period != "Нет")
                {
                    while (Count > SerialNumbers.Count)
                    {
                        SerialNumbers.Add(new SerialNumbers {IdProduct = Product.Id});
                    }

                    while (Count < SerialNumbers.Count)
                    {
                        SerialNumbers sr = SerialNumbers.FirstOrDefault(ser => string.IsNullOrEmpty(ser.Value)) ?? SerialNumbers.Last();
                        SerialNumbers.Remove(sr);
                    }

                    if (SerialNumbers.FirstOrDefault(objSer => string.IsNullOrEmpty(objSer.Value)) != null)
                        IsExpanded = true;
                }
                RaisePropertyChanged("IsValidate");
                RaisePropertyChanged();
            }
        }

        public decimal PurchasePrice
        {
            get => _purchaseInfo.PurchasePrice;
            set
            {
                _purchaseInfo.PurchasePrice = value;
                RaisePropertyChanged("IsValidate");
                RaisePropertyChanged();
            }
        }

        public int IdPurchaseReport
        {
            get => _purchaseInfo.IdPurchaseReport;
            set
            {
                _purchaseInfo.IdPurchaseReport = value;
                RaisePropertyChanged();
            }
        }

        public int IdProduct
        {
            get => _purchaseInfo.IdProduct;
            set
            {
                _purchaseInfo.IdProduct = value;
                RaisePropertyChanged();
            }
        }

        public int IdExchangeRate
        {
            get => _purchaseInfo.IdExchangeRate;
            set
            {
                _purchaseInfo.IdExchangeRate = value;
                RaisePropertyChanged();
            }
        }

        public virtual ExchangeRates ExchangeRate
        {
            get => _purchaseInfo.ExchangeRates;
            set
            {
                _purchaseInfo.ExchangeRates = value;
                RaisePropertyChanged();
            }
        }

        public override Products Product
        {
            get => _purchaseInfo.Products;
            set
            {
                _purchaseInfo.Products = value;
                SerialNumbers = new ObservableCollection<SerialNumbers>();
                RaisePropertyChanged();
            }
        }

        public virtual PurchaseReports PurchaseReport
        {
            get => _purchaseInfo.PurchaseReports;
            set
            {
                _purchaseInfo.PurchaseReports = value;
                RaisePropertyChanged();
            }
        }

        private ExchangeRates _exchangeRateOld;
        public ExchangeRates ExchangeRateOld
        {
            get => _exchangeRateOld;
            set
            {
                _exchangeRateOld = value;
                RaisePropertyChanged();
            }
        }

        private decimal _purchasePriceOld;
        public decimal PurchasePriceOld
        {
            get => _purchasePriceOld;
            set
            {
                _purchasePriceOld = value;
                RaisePropertyChanged();
            }
        }

        public override ObservableCollection<SerialNumbers> SerialNumbers
        {
            get => PurchaseInfo.SerialNumbers as ObservableCollection<SerialNumbers>;
            set
            {
                PurchaseInfo.SerialNumbers = value;
                SerialNumbers.CollectionChanged += SerialNumbersCollectionChanged;
                RaisePropertyChanged();
            }
        }

        public override bool IsValidate
        {
            get
            {
                if (Count == 0 || PurchasePrice <= 0 || SerialNumbers == null) return false;
                foreach (var serialNumber in SerialNumbers)
                {
                    if (string.IsNullOrEmpty(serialNumber.Value)) return false;
                }
                return true;
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public DelegateCommand<int?> ReloadSerialNumberCommand { get; }

        public PurchaseInfosViewModel()
        {
            PurchaseInfo = new PurchaseInfos{ Products = new Products() };
            ReloadSerialNumberCommand = new DelegateCommand<int?>(ReloadSerialNumber);
            SerialNumbers = new ObservableCollection<SerialNumbers>();
        }

        private void ReloadSerialNumber(int? obj)
        {
            if (obj == null) return;
            SerialNumbers[obj.Value].Value = "";
            RaisePropertyChanged("SerialNumbers");
        }
    }
}

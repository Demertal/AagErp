using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.ExchangeRate;
using ModelModul.PurchaseGoods;
using ModelModul.Store;
using ModelModul.Supplier;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace PurchaseGoodsModul.ViewModels
{
    public class RevaluationProductsFromPurchase: BindableBase
    {
        #region Properties

        private RevaluationProducts _revaluationProducts;
        public RevaluationProducts RevaluationProducts
        {
            get => _revaluationProducts;
            set
            {
                _revaluationProducts = value;
                if(_revaluationProducts != null) _revaluationProducts.PropertyChanged += (sender, args) => RaisePropertyChanged();
                RaisePropertyChanged();
            }
        }

        private ExchangeRates _exchangeRate;
        public ExchangeRates ExchangeRate
        {
            get => _exchangeRate;
            set
            {
                _exchangeRate = value;
                RaisePropertyChanged();
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                RaisePropertyChanged();
            }
        }

        private bool _isNotRevaluation;
        public bool IsNotRevaluation
        {
            get => _isNotRevaluation;
            set
            {
                _isNotRevaluation = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }

    public class PurchaseGoodsViewModel: BindableBase
    {
        #region Properties

        private readonly DbSetStores _dbSetStores = new DbSetStores();
        private readonly DbSetSuppliers _dbSetSuppliers = new DbSetSuppliers();
        private readonly DbSetExchangeRates _dbSetExchangeRates = new DbSetExchangeRates();

        private PurchaseReports _report = new PurchaseReports();
        private ObservableCollection<RevaluationProductsFromPurchase> _revaluationProducts = new ObservableCollection<RevaluationProductsFromPurchase>();

        public ObservableCollection<RevaluationProductsFromPurchase> RevaluationProducts
        {
            get => _revaluationProducts;
            set
            {
                _revaluationProducts = value;
                RaisePropertyChanged();
            }
        }

        private bool _allIsChecked;
        public bool AllIsChecked
        {
            get => _allIsChecked;
            set
            {
                _allIsChecked = value;
                RaisePropertyChanged();
                foreach (var rev in RevaluationProducts)
                {
                    rev.IsNotRevaluation = _allIsChecked;
                }
            }
        }

        public ObservableCollection<PurchaseInfos> PurchaseInfos => _report.PurchaseInfos as ObservableCollection<PurchaseInfos>;
        public ObservableCollection<Stores> Stores => _dbSetStores.List;
        public ObservableCollection<Suppliers> Suppliers => _dbSetSuppliers.List;
        public ObservableCollection<ExchangeRates> ExchangeRates => _dbSetExchangeRates.List;

        public bool IsEnabled
        {
            get
            {
                if(PurchaseInfos.Count == 0) return false;
                foreach (var purchaseInfo in PurchaseInfos)
                {
                    if (purchaseInfo.ExchangeRates.Title == "USD" && _report.Course <= 0) return false;

                    if (purchaseInfo.Count == 0 || purchaseInfo.PurchasePrice <= 0) return false;

                    if(purchaseInfo.Products == null) continue;
                    foreach (var serialNumber in purchaseInfo.Products.SerialNumbers)
                    {
                        if(string.IsNullOrEmpty(serialNumber.Value)) return false;
                    }
                }

                foreach (var revaluationProduct in RevaluationProducts)
                {
                    if (revaluationProduct.RevaluationProducts.NewSalesPrice <= 0 && !revaluationProduct.IsNotRevaluation) return false;
                }

                return true;
            }
        }

        private bool _isRevaluationProducts;
        public bool IsRevaluationProducts
        {
            get => _isRevaluationProducts;
            set
            {
                _isRevaluationProducts = value;
                RaisePropertyChanged();
            }
        }

        public Stores SelectedStore
        {
            get => _report.Stores;
            set
            {
                _report.Stores = value;
                RaisePropertyChanged();
            }
        }

        public Suppliers SelectedSuppliers
        {
            get => _report.Suppliers;
            set
            {
                _report.Suppliers = value;
                RaisePropertyChanged();
            }
        }

        public decimal Course
        {
            get => _report.Course;
            set
            {
                _report.Course = value;
                RaisePropertyChanged();
                PurchaseInfosViewModelPropertyChanged(this, new PropertyChangedEventArgs("Course"));
            }
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PurchaseInvoiceCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }

        #endregion

        public PurchaseGoodsViewModel()
        {
            AddProductPopupRequest = new InteractionRequest<INotification>();
            PurchaseInvoiceCommand = new DelegateCommand(PurchaseInvoice).ObservesCanExecute(() => IsEnabled);
            AddProductCommand = new DelegateCommand(AddProduct);
            RevaluationProducts.CollectionChanged += OnRevaluationProductsCollectionChanged;
            Load();
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            NewReport();
        }

        #region PropertyChanged

        private void OnRevaluationProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (RevaluationProductsFromPurchase item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= RevaluationProductsViewModelPropertyChanged;
                    }
                    if (RevaluationProducts.Count == 0) IsRevaluationProducts = false;
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (RevaluationProductsFromPurchase item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
            }

            RaisePropertyChanged("IsEnabled");
        }

        private void OnPurchaseInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (PurchaseInfos item in e.OldItems)
                    {
                        //Removed items
                        (item.Products.SerialNumbers as ObservableCollection<SerialNumbers>).CollectionChanged -=
                            OnSerialNumbersCollectionChanged;
                        item.PropertyChanged -= PurchaseInfosViewModelPropertyChanged;
                        RevaluationProductsFromPurchase rev = RevaluationProducts.FirstOrDefault(ob => ob.RevaluationProducts.IdProduct == item.IdProduct);
                        RevaluationProducts.Remove(rev);
                    }

                    if (RevaluationProducts.Count == 0) IsRevaluationProducts = false;
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (PurchaseInfos item in e.NewItems)
                    {
                        (item.Products.SerialNumbers as ObservableCollection<SerialNumbers>).CollectionChanged +=
                            OnSerialNumbersCollectionChanged;
                        //Added items
                        item.PropertyChanged += PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("IsEnabled");
        }

        private void OnSerialNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SerialNumbers item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SerialNumbersViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumbers item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SerialNumbersViewModelPropertyChanged;
                    }

                    break;
            }

            RaisePropertyChanged("IsEnabled");
        }

        private void RevaluationProductsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("RevaluationProducts");
            RaisePropertyChanged("IsEnabled");
        }

        private void SerialNumbersViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("IsEnabled");
        }

        private void PurchaseInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var purchaseInfo in PurchaseInfos)
            {
                if (purchaseInfo.Products.WarrantyPeriods.Period != "Нет")
                {
                    while (purchaseInfo.Count > purchaseInfo.Products.SerialNumbers.Count)
                    {
                        purchaseInfo.Products.SerialNumbers.Add(new SerialNumbers { IdProduct = purchaseInfo.Products.Id });
                    }
                    while (purchaseInfo.Count < purchaseInfo.Products.SerialNumbers.Count)
                    {
                        SerialNumbers sr = purchaseInfo.Products.SerialNumbers.FirstOrDefault(ser => string.IsNullOrEmpty(ser.Value)) ??
                                           purchaseInfo.Products.SerialNumbers.First();
                        purchaseInfo.Products.SerialNumbers.Remove(sr);
                    }
                }

                if ((double)(purchaseInfo.Products.PurchasePrice - purchaseInfo.PurchasePrice) <= 0.001 &&
                    purchaseInfo.Products.ExchangeRates.Id == purchaseInfo.ExchangeRates.Id)
                {
                    if (purchaseInfo.Products.SalesPrice != 0)
                    {
                        if (RevaluationProducts.Count(
                                rev => rev.RevaluationProducts.IdProduct == purchaseInfo.IdProduct) != 0)
                        {
                            int idx = RevaluationProducts.IndexOf(RevaluationProducts.FirstOrDefault(rev =>
                                rev.RevaluationProducts.IdProduct == purchaseInfo.IdProduct));
                            RevaluationProducts.RemoveAt(idx);
                        }
                        continue;
                    }
                    
                }
                if (RevaluationProducts.Count(rev => rev.RevaluationProducts.IdProduct == purchaseInfo.IdProduct) == 0)
                {
                    RevaluationProducts.Add(new RevaluationProductsFromPurchase
                    {
                        ExchangeRate = purchaseInfo.ExchangeRates,
                        IsNotRevaluation = false,
                        PurchasePrice = purchaseInfo.PurchasePrice,
                        RevaluationProducts = new RevaluationProducts
                        {
                            IdProduct = purchaseInfo.IdProduct,
                            OldSalesPrice = purchaseInfo.Products.SalesPrice,
                            Products = purchaseInfo.Products
                        }
                    });
                }
                else
                {
                    int idx = RevaluationProducts.IndexOf(RevaluationProducts.FirstOrDefault(rev =>
                        rev.RevaluationProducts.IdProduct == purchaseInfo.IdProduct));
                    if (idx >= 0)
                    {
                        RevaluationProducts[idx].PurchasePrice = purchaseInfo.PurchasePrice;
                        RevaluationProducts[idx].ExchangeRate = purchaseInfo.ExchangeRates;
                    }
                }
                IsRevaluationProducts = true;
            }
            RaisePropertyChanged("IsEnabled");
            RaisePropertyChanged("PurchaseInfos");
            RaisePropertyChanged("RevaluationProducts");
        }

        #endregion

        private async void Load()
        {
            try
            {
                await _dbSetStores.Load();
                RaisePropertyChanged("Stores");
                await _dbSetExchangeRates.Load();
                RaisePropertyChanged("ExchangeRates");
                await _dbSetSuppliers.Load();
                RaisePropertyChanged("Suppliers");
                Course = _dbSetExchangeRates.List.First(e => e.Title == "USD").Course;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewReport()
        {
            _report = new PurchaseReports
            {
                PurchaseInfos = new ObservableCollection<PurchaseInfos>()
            };
            RevaluationProducts.Clear();
            PurchaseInfos.CollectionChanged += OnPurchaseInfosCollectionChanged;
            Course = _dbSetExchangeRates.List.FirstOrDefault(e => e.Title == "USD") != null ? _dbSetExchangeRates.List.First(e => e.Title == "USD").Course : 0;
            RaisePropertyChanged("PurchaseInfos");
            RaisePropertyChanged("Course");
            RaisePropertyChanged("IsEnabled");
            RaisePropertyChanged("RevaluationProducts");
            IsRevaluationProducts = false;
        }

        #region DelegateCommand

        private void PurchaseInvoice()
        {
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о закупке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetPurchaseGoods dbSetPurchase = new DbSetPurchaseGoods();
                List<RevaluationProducts> revaluationProducts = new List<RevaluationProducts>();
                foreach (var revaluationProduct in RevaluationProducts)
                {
                    if (!revaluationProduct.IsNotRevaluation)
                    {
                        revaluationProducts.Add(revaluationProduct.RevaluationProducts);
                    }
                }
                dbSetPurchase.Add((PurchaseReports)_report.Clone(), revaluationProducts);
                MessageBox.Show("Отчет о закупке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            AddProductPopupRequest.Raise(new Confirmation { Title = "Выбрать товар" }, Callback);
        }

        private void DeleteProduct(Collection<object> obj)
        {
            List<PurchaseInfos> list = obj.Cast<PurchaseInfos>().ToList();
            list.ForEach(item => PurchaseInfos.Remove(item));
            RaisePropertyChanged("PurchaseInfos");
        }

        private void Callback(INotification obj)
        {
            if(obj.Content == null) return;
            foreach (Products product in (IEnumerable)obj.Content)
            {
                if(PurchaseInfos.Count(p => p.Products.Id == product.Id) != 0) continue;
                PurchaseInfos.Add(new PurchaseInfos
                {
                    Products = new Products
                    {
                        Barcode = product.Barcode,
                        Groups = product.Groups,
                        ExchangeRates = product.ExchangeRates,
                        Id = product.Id,
                        IdExchangeRate = product.IdExchangeRate,
                        IdGroup = product.IdGroup,
                        IdUnitStorage = product.IdUnitStorage,
                        IdWarrantyPeriod = product.IdWarrantyPeriod,
                        PurchasePrice = product.PurchasePrice,
                        SalesPrice = product.SalesPrice,
                        VendorCode = product.VendorCode,
                        UnitStorages = product.UnitStorages,
                        WarrantyPeriods = product.WarrantyPeriods,
                        Title = product.Title,
                        SerialNumbers = new ObservableCollection<SerialNumbers>()
                    },
                    IdProduct = product.Id,
                    IdExchangeRate = product.IdExchangeRate,
                    ExchangeRates = product.ExchangeRates
                });
            }
            RaisePropertyChanged("PurchaseInfos");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ModelModul;
using ModelModul.Counterparty;
using ModelModul.ExchangeRate;
using ModelModul.Product;
using ModelModul.PurchaseGoods;
using ModelModul.Store;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace PurchaseGoodsModul.ViewModels
{
    public class PurchaseGoodsViewModel: ViewModelBase
    {
        #region Properties

        private string _barcode;

        private readonly IRegionManager _regionManager;

        private PurchaseReports _report = new PurchaseReports();

        private ObservableCollection<PurchaseInfosViewModel> _purchaseInfos = new ObservableCollection<PurchaseInfosViewModel>();
        public ObservableCollection<PurchaseInfosViewModel> PurchaseInfos
        {
            get => _purchaseInfos;
            set => SetProperty(ref _purchaseInfos, value);
        }

        private ObservableCollection<Stores> _stores = new ObservableCollection<Stores>();
        public ObservableCollection<Stores> Stores
        {
            get => _stores;
            set
            {
                SetProperty(ref _stores, value);
                SelectedStore = Stores.FirstOrDefault();
            }
        }

        private ObservableCollection<Counterparties> _suppliers = new ObservableCollection<Counterparties>();
        public ObservableCollection<Counterparties> Suppliers
        {
            get => _suppliers;
            set
            {
                SetProperty(ref _suppliers, value);
                SelectedSupplier = Suppliers.FirstOrDefault();
            }
        }

        private ObservableCollection<ExchangeRates> _exchangeRates = new ObservableCollection<ExchangeRates>();
        public ObservableCollection<ExchangeRates> ExchangeRates
        {
            get => _exchangeRates;
            set => SetProperty(ref _exchangeRates, value);
        }

        public bool IsValidate
        {
            get
            {
                if(PurchaseInfos.Count == 0) return false;
                foreach (var purchaseInfo in PurchaseInfos)
                {
                    if (purchaseInfo.ExchangeRate.Title == "USD" && _report.Course <= 0) return false;

                    if(!purchaseInfo.IsValidate) return false;
                }
                return true;
            }
        }

        public Stores SelectedStore
        {
            get => _report.Stores;
            set
            {
                _report.Stores = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public Counterparties SelectedSupplier
        {
            get => _report.Counterparties;
            set
            {
                _report.Counterparties = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public decimal Course
        {
            get => _report.Course;
            set
            {
                _report.Course = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
                PurchaseInfosViewModelPropertyChanged(this, new PropertyChangedEventArgs("Course"));
            }
        }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var purchaseInfo in PurchaseInfos)
                {
                    if (purchaseInfo.ExchangeRate.Title == "ГРН")
                    {
                        total += purchaseInfo.PurchasePrice * purchaseInfo.Count;
                    }
                    else
                    {
                        total += purchaseInfo.PurchasePrice * purchaseInfo.Count * Course;
                    }
                }

                return total;
            }
        }
        
        public string TextInfo
        {
            get => _report.TextInfo;
            set
            {
                _report.TextInfo = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PurchaseInvoiceCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }

        #endregion

        public PurchaseGoodsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddProductPopupRequest = new InteractionRequest<INotification>();
            PurchaseInvoiceCommand = new DelegateCommand(PurchaseInvoice).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            PurchaseInfos.CollectionChanged += OnPurchaseInfosCollectionChanged;
            NewReport();
        }

        #region PropertyChanged

        private void OnPurchaseInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (PurchaseInfosViewModel item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (PurchaseInfosViewModel item in e.NewItems)
                    {
                       //Added items
                        item.PropertyChanged += PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
            }
            _barcode = "";
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("Total");
        }

        private void PurchaseInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _barcode = "";
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("PurchaseInfos");
            RaisePropertyChanged("Total");
        }

        #endregion

        private void Load()
        {
            try
            {
                DbSetStores dbSetStores = new DbSetStores();
                Stores = dbSetStores.Load();
                DbSetExchangeRates dbSetExchange = new DbSetExchangeRates();
                ExchangeRates = dbSetExchange.Load();
                RaisePropertyChanged("ExchangeRates");
                DbSetCounterparties dbSetSuppliers = new DbSetCounterparties();
                Suppliers = dbSetSuppliers.Load(TypeCounterparties.Suppliers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewReport()
        {
            Load();
            _report = new PurchaseReports();
            SelectedStore = Stores.FirstOrDefault();
            SelectedSupplier = Suppliers.FirstOrDefault();
            PurchaseInfos.Clear();
            Course = ExchangeRates.FirstOrDefault(e => e.Title == "USD")?.Course ?? 0;
            RaisePropertyChanged("PurchaseInfos");
            RaisePropertyChanged("TextInfo");
            RaisePropertyChanged("Course");
            RaisePropertyChanged("Total");
        }

        private void Navigate(List<Products> revaluationProducts)
        {
            NavigationParameters navigationParameters =
                new NavigationParameters {{"RevaluationProducts", revaluationProducts}};
            _regionManager.RequestNavigate("ContentRegion", "RevaluationProductsView", navigationParameters);
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
                _report.PurchaseInfos = new List<PurchaseInfos>();
                foreach (var purchaseInfo in PurchaseInfos)
                {
                    _report.PurchaseInfos.Add(purchaseInfo.PurchaseInfo);
                    ((List<PurchaseInfos>) _report.PurchaseInfos)[_report.PurchaseInfos.Count-1].Products =
                        (Products) purchaseInfo.Product.Clone();
                }
                DbSetPurchaseGoods dbSet = new DbSetPurchaseGoods();
                dbSet.Add((PurchaseReports)_report.Clone());
                MessageBox.Show("Отчет о закупке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (PurchaseInfos.FirstOrDefault(objPr =>
                        objPr.Product.SalesPrice == 0 ||
                        Math.Abs(objPr.PurchasePrice - objPr.PurchasePriceOld) > (decimal) 0.01 ||
                        objPr.ExchangeRate.Id != objPr.ExchangeRateOld.Id) != null) 
                {
                    if (MessageBox.Show(
                            "Есть товар на который изменилась цена закупки. Вы хотите переоценить этот товар?",
                            "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes)
                    {
                        NewReport();
                        return;
                    }
                    List<Products> revaluationProducts = PurchaseInfos
                        .Where(objPr =>
                            Math.Abs(objPr.PurchasePrice - objPr.PurchasePriceOld) > (decimal) 0.01 ||
                            objPr.ExchangeRate.Id != objPr.ExchangeRateOld.Id).Select(objPr => objPr.Product).ToList();
                    Navigate(revaluationProducts);
                }
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
            List<PurchaseInfosViewModel> list = obj.Cast<PurchaseInfosViewModel>().ToList();
            list.ForEach(item => PurchaseInfos.Remove(item));
        }

        private void Callback(INotification obj)
        {
            if(!(obj.Content is Products)) return;
            try
            {
                AddProduct((Products)obj.Content);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListenKeyboard(KeyEventArgs obj)
        {
            if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
            {
                _barcode += obj.Key.ToString()[1].ToString();
            }
            else if (obj.Key >= Key.A && obj.Key <= Key.Z)
            {
                _barcode += obj.Key.ToString();
            }
            else if (obj.Key == Key.Enter)
            {
                try
                {
                    if(string.IsNullOrEmpty(_barcode) || _barcode.Length < 8 || _barcode.Length > 13)
                    {
                        _barcode = "";
                        return;
                    }
                    DbSetProducts dbSetProducts = new DbSetProducts();
                    Products product = dbSetProducts.FindProductByBarcode(_barcode);
                    if (product == null) throw new Exception("Товар не найден");
                    AddProduct(product);
                }
                catch (Exception ex)
                {
                    _barcode = "";
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                _barcode = "";
            }
            else
            {
                _barcode = "";
            }
        }

        private void AddProduct(Products product)
        {
            DbSetProducts dbSetProducts = new DbSetProducts();
            PurchaseStruct purchaseStruct = dbSetProducts.GetPurchasePrice(product.Id);
            PurchaseInfosViewModel purchaseInfo = PurchaseInfos.FirstOrDefault(oblPur => oblPur.PurchaseInfo.IdProduct == product.Id);
            if (purchaseStruct?.ExchangeRate == null)
            {
                if (purchaseStruct == null)
                {
                    purchaseStruct = new PurchaseStruct{PurchasePrice = 0};
                }
                purchaseStruct.ExchangeRate = purchaseStruct?.ExchangeRate == null
                    ? ExchangeRates.FirstOrDefault(objEx => objEx.Title == "ГРН")
                    : ExchangeRates.FirstOrDefault(objEx => objEx.Id == purchaseStruct.ExchangeRate.Id);
            }
            PurchaseInfosViewModel temp = new PurchaseInfosViewModel
            {
                IdProduct = product.Id,
                ExchangeRate = purchaseStruct.ExchangeRate,
                IdExchangeRate = purchaseStruct.ExchangeRate.Id,
                PurchasePrice = 0,
                Count = 0,
                Product = (Products)product.Clone(),
                ExchangeRateOld = purchaseStruct.ExchangeRate,
                PurchasePriceOld = purchaseStruct.PurchasePrice
            };
            if (purchaseInfo == null)
            {
                PurchaseInfos.Add(temp);
            }
            else
            {
                purchaseInfo.Count++;
            }
            RaisePropertyChanged("PurchaseInfos");
        }

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}

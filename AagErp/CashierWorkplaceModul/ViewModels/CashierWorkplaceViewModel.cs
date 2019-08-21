//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Windows;
//using System.Windows.Input;
//using ModelModul;
//using ModelModul.Counterparty;
//using ModelModul.Product;
//using ModelModul.SerialNumber;
//using ModelModul.ArrivalStore;
//using Prism.Commands;
//using Prism.Interactivity.InteractionRequest;
//using Prism.Regions;

//namespace CashierWorkplaceModul.ViewModels
//{
//    class CashierWorkplaceViewModel: ViewModelBase
//    {
//        #region Properties

//        private string _barcode;
//        private string _serialNumber;

//        private readonly SalesReports _salesReports = new SalesReports();

//        private ObservableCollection<SalesInfosViewModel> _salesInfos = new ObservableCollection<SalesInfosViewModel>();

//        public ObservableCollection<SalesInfosViewModel> SalesInfos
//        {
//            get => _salesInfos;
//            set => SetProperty(ref _salesInfos, value);
//        }

//        private ObservableCollection<ArrivalStore> _stores = new ObservableCollection<ArrivalStore>();

//        public ObservableCollection<ArrivalStore> ArrivalStore
//        {
//            get => _stores;
//            set
//            {
//                _stores = value;
//                RaisePropertyChanged();
//            }
//        }

//        private ObservableCollection<Counterparty> _customers = new ObservableCollection<Counterparty>();
//        public ObservableCollection<Counterparty> Customers
//        {
//            get => _customers;
//            set
//            {
//                _customers = value;
//                RaisePropertyChanged();
//            }
//        }

//        public bool IsEnabledAddProduct
//        {
//            get
//            {
//                return SalesInfos.FirstOrDefault(objSale =>
//                           objSale.WarrantyPeriod.Period != "Нет" && objSale.SerialNumber.Id == 0) == null;
//            }
//        }

//        public bool IsEnabledSale
//        {
//            get
//            {
//                return SalesInfos.Count != 0 && SalesInfos.All(sal => sal.IsValidate);
//            }
//        }

//        public int IdStore
//        {
//            get => _salesReports.IdStore;
//            set
//            {
//                _salesReports.IdStore = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int IdCustomer
//        {
//            get => _salesReports.IdCounterparty;
//            set
//            {
//                _salesReports.IdCounterparty = value;
//                RaisePropertyChanged();
//            }
//        }

//        public decimal Total
//        {
//            get
//            {
//                decimal sum = 0;
//                foreach (var salesInfo in SalesInfos)
//                {
//                    sum += salesInfo.Count * salesInfo.SellingPrice;
//                }

//                return sum;
//            }
//        }

//        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

//        public DelegateCommand AddProductCommand { get; }
//        public DelegateCommand SaleCommand { get; }
//        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
//        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }
//        public DelegateCommand<KeyEventArgs> ListenKeyboardSerialNumbersCommand { get; }
//        #endregion

//        public CashierWorkplaceViewModel()
//        {
//            Load();
//            AddProductPopupRequest = new InteractionRequest<INotification>();
//            SaleCommand = new DelegateCommand(Sale).ObservesCanExecute(() => IsEnabledSale);
//            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabledAddProduct);
//            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
//            _salesReports.SalesInfos = new ObservableCollection<SalesInfos>();
//            SalesInfos.CollectionChanged += OnSalesInfosCollectionChanged;
//            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
//            ListenKeyboardSerialNumbersCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboardSerialNumbers);
//            NewReport();
//        }

//        #region PropertyChanged

//        private void OnSalesInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Remove:
//                    foreach (SalesInfosViewModel item in e.OldItems)
//                    {
//                        //Removed items
//                        item.PropertyChanged -= SalesInfosViewModelPropertyChanged;
//                    }
//                    break;
//                case NotifyCollectionChangedAction.AddAsync:
//                    foreach (SalesInfosViewModel item in e.NewItems)
//                    {
//                        //Added items
//                        item.PropertyChanged += SalesInfosViewModelPropertyChanged;
//                    }
//                    break;
//            }
//            RaisePropertyChanged("SalesInfos");
//            RaisePropertyChanged("IsEnabledSale");
//            RaisePropertyChanged("IsEnabledAddProduct");
//            RaisePropertyChanged("Total");
//        }

//        private void SalesInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            _serialNumber = "";
//            RaisePropertyChanged("SalesInfos");
//            RaisePropertyChanged("IsEnabledSale");
//            RaisePropertyChanged("IsEnabledAddProduct");
//            RaisePropertyChanged("Total");
//        }

//        #endregion

//        private void Load()
//        {
//            try
//            {
//                SqlStoreRepository dbSetStores = new SqlStoreRepository();
//                ArrivalStore = dbSetStores.Load();
//                IdStore = ArrivalStore.FirstOrDefault()?.Id ?? 0;
//                SQLCounterpartyRepository dbSetCounterparties = new SQLCounterpartyRepository();
//                Customers = dbSetCounterparties.Load(TypeCounterparties.Buyers);
//                IdCustomer = Customers.FirstOrDefault(objCus => objCus.Title == "Покупатель")?.Id ?? 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private void NewReport()
//        {
//            Load();
//            SalesInfos.Clear();
//            RaisePropertyChanged("IsEnabledSale");
//            RaisePropertyChanged("IsEnabledAddProduct");
//            RaisePropertyChanged("SalesInfos");
//        }

//        #region DelegateCommand

//        private void Sale()
//        {
//            if (MessageBox.Show(
//                    "Вы уверены, что хотите провести продажу? Этот отчет невозможно будет изменить после.",
//                    "Проведение продажи", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
//                MessageBoxResult.Yes) return;
//            try
//            {
//                _salesReports.SalesInfos = new List<SalesInfos>();
//                foreach (var purchaseInfo in SalesInfos)
//                {
//                    _salesReports.SalesInfos.AddAsync(purchaseInfo.SalesInfo);
//                    ((List<SalesInfos>)_salesReports.SalesInfos)[_salesReports.SalesInfos.Count - 1].Product =
//                        (Product)purchaseInfo.Product.Clone();
//                }
//                DbSetSalesGoods dbSet = new DbSetSalesGoods();
//                dbSet.AddAsync((SalesReports)_salesReports.Clone());
//                MessageBox.Show("Продажа добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
//                NewReport();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private void AddProduct()
//        {
//            AddProductPopupRequest.Raise(new Confirmation { Title = "Выбрать товар" }, Callback);
//        }

//        private void DeleteProduct(Collection<object> obj)
//        {
//            List<SalesInfosViewModel> list = obj.Cast<SalesInfosViewModel>().ToList();
//            list.ForEach(item => SalesInfos.Remove(item));
//        }

//        private void Callback(INotification obj)
//        {
//            if (obj.Content == null) return;
//            SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale =>
//                objSale.Product.Id == ((Product) obj.Content).Id && objSale.WarrantyPeriod.Period == "Нет");
//            if (sale != null)
//            {
//                sale.Count++;
//            }
//            else
//            {
//                SalesInfos.AddAsync(new SalesInfosViewModel
//                {
//                    IdProduct = ((Product)obj.Content).Id,
//                    Product = (Product)obj.Content,
//                    SellingPrice = ((Product)obj.Content).SalesPrice,
//                    Count = 1,
//                    SerialNumber = new SerialNumber()
//                });
//            }
//        }

//        private void ListenKeyboard(KeyEventArgs obj)
//        {
//            if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
//            {
//                _barcode += obj.Key.ToString()[1].ToString();
//            }
//            else if (obj.Key >= Key.A && obj.Key <= Key.Z)
//            {
//                _barcode += obj.Key.ToString();
//            }
//            else if (obj.Key == Key.Enter)
//            {
//                try
//                {
//                    SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
//                    ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_barcode);

//                    SerialNumber freeSerialNumbers = null;
//                    foreach (var objSer in serialNumberses)
//                    {
//                        if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
//                        freeSerialNumbers = objSer;
//                        break;
//                    }

//                    SalesInfosViewModel saleInfos = SalesInfos.FirstOrDefault(objSale =>
//                        objSale.Product.WarrantyPeriod.Period != "Нет" && objSale.SerialNumber.Id == 0);

//                    if (freeSerialNumbers == null && saleInfos != null)
//                        throw new Exception("Сначала нужно указать серийный номер к товару: \"" +
//                                            saleInfos.Product.Title +
//                                            "\".\nЕсли это был серийный номер попробуйте указать его еще раз или используйте другой серийный номер, или ввести вручную");
//                    if (freeSerialNumbers != null && saleInfos != null)
//                    {
//                        saleInfos.SerialNumber = freeSerialNumbers;
//                        _barcode = "";
//                        return;
//                    }
//                    SqlProductRepository dbSetProducts = new SqlProductRepository();
//                    Product product = null;
//                    if (freeSerialNumbers != null)
//                    {
//                        product = dbSetProducts.FindProductById(freeSerialNumbers.IdProduct);
//                        if(product == null) throw new Exception("Товар не найден");
//                        SalesInfos.AddAsync(new SalesInfosViewModel
//                        {
//                            IdProduct = product.Id,
//                            Product = product,
//                            SellingPrice = product.SalesPrice,
//                            Count = 1,
//                            SerialNumber = freeSerialNumbers
//                        });
//                    }

//                    if (freeSerialNumbers == null)
//                    {
//                        if(serialNumberses.Count != 0) throw new Exception("Нет свободного серийного номера: " + _barcode);
//                        product = dbSetProducts.FindProductByBarcode(_barcode);
//                        if (product == null) throw new Exception("Товар не найден");
//                        saleInfos = SalesInfos.FirstOrDefault(objSale => objSale.IdProduct == product.Id);
//                        SalesInfosViewModel temp = new SalesInfosViewModel
//                        {
//                            IdProduct = product.Id,
//                            Product = product,
//                            SellingPrice = product.SalesPrice,
//                            Count = 1,
//                            SerialNumber = new SerialNumber()
//                        };
//                        if (saleInfos == null)
//                        {
//                            SalesInfos.AddAsync(temp);
//                        }
//                        else
//                        {
//                            if (product.WarrantyPeriod.Period == "Нет") saleInfos.Count++;
//                            else SalesInfos.AddAsync(temp);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _barcode = "";
//                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//                _barcode = "";
//            }
//            else
//            {
//                _barcode = "";
//            }
//        }

//        private void ListenKeyboardSerialNumbers(KeyEventArgs obj)
//        {
//            if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
//            {
//                _serialNumber += obj.Key.ToString()[1].ToString();
//            }
//            else if (obj.Key >= Key.A && obj.Key <= Key.Z)
//            {
//                _serialNumber += obj.Key.ToString();
//            }
//            else if (obj.Key == Key.Enter)
//            {
//                try
//                {
//                    SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == 0 && objSale.Product.WarrantyPeriod.Period != "Нет");
//                    if (sale == null) throw new Exception("Возникла непредвиденная ошибка");
//                    SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
//                    ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_serialNumber, sale.IdProduct);
//                    if(serialNumberses.Count == 0) throw new Exception("Cерийный номер: \"" + _serialNumber + "\" для этого товара не найден: ");
//                    SerialNumber freeSerialNumbers = null;
//                    foreach (var objSer in serialNumberses)
//                    {
//                        if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
//                        freeSerialNumbers = objSer;
//                        break;
//                    }
//                    sale.SerialNumber = freeSerialNumbers ?? throw new Exception("Не свободного серийного номера: " + _serialNumber);
//                    _serialNumber = "";
//                    _barcode = "";
//                }
//                catch (Exception ex)
//                {
//                    _barcode = "";
//                    _serialNumber = "";
//                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//            }
//            else
//            {
//                _barcode = "";
//                _serialNumber = "";
//            }
//        }

//        #endregion

//        #region INavigationAware

//        public override void OnNavigatedTo(NavigationContext navigationContext)
//        {
//        }

//        public override bool IsNavigationTarget(NavigationContext navigationContext)
//        {
//            return true;
//        }

//        public override void OnNavigatedFrom(NavigationContext navigationContext)
//        {
//        }

//        #endregion
//    }
//}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModelModul;
using ModelModul.Counterparty;
using ModelModul.Product;
using ModelModul.SalesGoods;
using ModelModul.SerialNumber;
using ModelModul.Store;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace CashierWorkplaceModul.ViewModels
{
    class CashierWorkplaceViewModel: BindableBase
    {
        #region Properties

        private string _barcode;
        private string _serialNumber;

        private readonly SalesReports _salesReports = new SalesReports();

        private ObservableCollection<SalesInfosViewModel> _salesInfos = new ObservableCollection<SalesInfosViewModel>();

        public ObservableCollection<SalesInfosViewModel> SalesInfos
        {
            get => _salesInfos;
            set => SetProperty(ref _salesInfos, value);
        }

        private ObservableCollection<Stores> _stores = new ObservableCollection<Stores>();

        public ObservableCollection<Stores> Stores
        {
            get => _stores;
            set
            {
                _stores = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Counterparties> _customers = new ObservableCollection<Counterparties>();
        public ObservableCollection<Counterparties> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnabledAddProduct
        {
            get
            {
                return SalesInfos.FirstOrDefault(objSale =>
                           objSale.WarrantyPeriod.Period != "Нет" && objSale.SerialNumber.Id == 0) == null;
            }
        }

        public bool IsEnabledSale
        {
            get
            {
                return SalesInfos.Count != 0 && SalesInfos.All(sal => sal.IsValidate);
            }
        }

        public int IdStore
        {
            get => _salesReports.IdStore;
            set
            {
                _salesReports.IdStore = value;
                RaisePropertyChanged();
            }
        }

        public int IdCustomer
        {
            get => _salesReports.IdCounterparty;
            set
            {
                _salesReports.IdCounterparty = value;
                RaisePropertyChanged();
            }
        }

        public decimal Total
        {
            get
            {
                decimal sum = 0;
                foreach (var salesInfo in SalesInfos)
                {
                    sum += salesInfo.Count * salesInfo.SellingPrice;
                }

                return sum;
            }
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand SaleCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardSerialNumbersCommand { get; }
        #endregion

        public CashierWorkplaceViewModel()
        {
            LoadAsync();
            AddProductPopupRequest = new InteractionRequest<INotification>();
            SaleCommand = new DelegateCommand(Sale).ObservesCanExecute(() => IsEnabledSale);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabledAddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            _salesReports.SalesInfos = new ObservableCollection<SalesInfos>();
            SalesInfos.CollectionChanged += OnSalesInfosCollectionChanged;
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            ListenKeyboardSerialNumbersCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboardSerialNumbers);
            NewReport();
        }

        #region PropertyChanged

        private void OnSalesInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SalesInfosViewModel item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SalesInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SalesInfosViewModel item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SalesInfosViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("SalesInfos");
            RaisePropertyChanged("IsEnabledSale");
            RaisePropertyChanged("IsEnabledAddProduct");
            RaisePropertyChanged("Total");
        }

        private void SalesInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _serialNumber = "";
            RaisePropertyChanged("SalesInfos");
            RaisePropertyChanged("IsEnabledSale");
            RaisePropertyChanged("IsEnabledAddProduct");
            RaisePropertyChanged("Total");
        }

        #endregion

        private async Task LoadAsync()
        {
            try
            {
                DbSetStores dbSetStores = new DbSetStores();
                Stores = await dbSetStores.LoadAsync();
                IdStore = Stores.FirstOrDefault()?.Id ?? 0;
                DbSetCounterparties dbSetCounterparties = new DbSetCounterparties();
                Customers = await dbSetCounterparties.LoadAsync(TypeCounterparties.Buyers);
                IdCustomer = Customers.FirstOrDefault(objCus => objCus.Title == "Покупатель")?.Id ?? 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewReport()
        {
            LoadAsync();
            SalesInfos.Clear();
            RaisePropertyChanged("IsEnabledSale");
            RaisePropertyChanged("IsEnabledAddProduct");
            RaisePropertyChanged("SalesInfos");
        }

        #region DelegateCommand

        private async void Sale()
        {
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести продажу? Этот отчет невозможно будет изменить после.",
                    "Проведение продажи", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                _salesReports.SalesInfos = new List<SalesInfos>();
                foreach (var purchaseInfo in SalesInfos)
                {
                    _salesReports.SalesInfos.Add(purchaseInfo.SalesInfo);
                    ((List<SalesInfos>)_salesReports.SalesInfos)[_salesReports.SalesInfos.Count - 1].Products =
                        (Products)purchaseInfo.Product.Clone();
                }
                DbSetSalesGoods dbSet = new DbSetSalesGoods();
                await dbSet.AddAsync((SalesReports)_salesReports.Clone());
                MessageBox.Show("Продажа добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
            List<SalesInfosViewModel> list = obj.Cast<SalesInfosViewModel>().ToList();
            list.ForEach(item => SalesInfos.Remove(item));
        }

        private void Callback(INotification obj)
        {
            if (obj.Content == null) return;
            SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale =>
                objSale.Product.Id == ((Products) obj.Content).Id && objSale.WarrantyPeriod.Period == "Нет");
            if (sale != null)
            {
                sale.Count++;
            }
            else
            {
                SalesInfos.Add(new SalesInfosViewModel
                {
                    IdProduct = ((Products)obj.Content).Id,
                    Product = (Products)obj.Content,
                    SellingPrice = ((Products)obj.Content).SalesPrice,
                    Count = 1,
                    SerialNumber = new SerialNumbers()
                });
            }
        }

        private async void ListenKeyboard(KeyEventArgs obj)
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
                    DbSetSerialNumbers dbSetSerialNumbers = new DbSetSerialNumbers();
                    ObservableCollection<SerialNumbers> serialNumberses = await dbSetSerialNumbers.FindFreeSerialNumbersAsync(_barcode);

                    SerialNumbers freeSerialNumbers = null;
                    foreach (var objSer in serialNumberses)
                    {
                        if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
                        freeSerialNumbers = objSer;
                        break;
                    }

                    SalesInfosViewModel saleInfos = SalesInfos.FirstOrDefault(objSale =>
                        objSale.Product.WarrantyPeriods.Period != "Нет" && objSale.SerialNumber.Id == 0);

                    if (freeSerialNumbers == null && saleInfos != null)
                        throw new Exception("Сначала нужно указать серийный номер к товару: \"" +
                                            saleInfos.Product.Title +
                                            "\".\nЕсли это был серийный номер попробуйте указать его еще раз или используйте другой серийный номер или ввести вручную");
                    if (freeSerialNumbers != null && saleInfos != null)
                    {
                        saleInfos.SerialNumber = freeSerialNumbers;
                        _barcode = "";
                        return;
                    }
                    DbSetProducts dbSetProducts = new DbSetProducts();
                    Products product = null;
                    if (freeSerialNumbers != null)
                    {
                        product = await ((DbSetProducts) dbSetProducts).FindProductByIdAsync(freeSerialNumbers.IdProduct);
                        if(product == null) throw new Exception("Товар не найден");
                        SalesInfos.Add(new SalesInfosViewModel
                        {
                            IdProduct = product.Id,
                            Product = product,
                            SellingPrice = product.SalesPrice,
                            Count = 1,
                            SerialNumber = freeSerialNumbers
                        });
                    }

                    if (freeSerialNumbers == null)
                    {
                        if(serialNumberses.Count != 0) throw new Exception("Не свободного серийного номера: " + _barcode);
                        product = await ((DbSetProducts)dbSetProducts).FindProductByBarcodeAsync(_barcode);
                        if (product == null) throw new Exception("Товар не найден");
                        saleInfos = SalesInfos.FirstOrDefault(objSale => objSale.IdProduct == product.Id);
                        SalesInfosViewModel temp = new SalesInfosViewModel
                        {
                            IdProduct = product.Id,
                            Product = product,
                            SellingPrice = product.SalesPrice,
                            Count = 1,
                            SerialNumber = new SerialNumbers()
                        };
                        if (saleInfos == null)
                        {
                            SalesInfos.Add(temp);
                        }
                        else
                        {
                            if (product.WarrantyPeriods.Period == "Нет") saleInfos.Count++;
                            else SalesInfos.Add(temp);
                        }
                    }
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

        private async void ListenKeyboardSerialNumbers(KeyEventArgs obj)
        {
            if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
            {
                _serialNumber += obj.Key.ToString()[1].ToString();
            }
            else if (obj.Key >= Key.A && obj.Key <= Key.Z)
            {
                _serialNumber += obj.Key.ToString();
            }
            else if (obj.Key == Key.Enter)
            {
                try
                {
                    SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == 0 && objSale.Product.WarrantyPeriods.Period != "Нет");
                    if (sale == null) throw new Exception("Возникла непредвиденная ошибка");
                    DbSetSerialNumbers dbSetSerialNumbers = new DbSetSerialNumbers();
                    ObservableCollection<SerialNumbers> serialNumberses = await dbSetSerialNumbers.FindFreeSerialNumbersAsync(_serialNumber, sale.IdProduct);
                    if(serialNumberses.Count == 0) throw new Exception("Cерийный номер: \"" + _serialNumber + "\" для этого товара не найден: ");
                    SerialNumbers freeSerialNumbers = null;
                    foreach (var objSer in serialNumberses)
                    {
                        if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
                        freeSerialNumbers = objSer;
                        break;
                    }
                    sale.SerialNumber = freeSerialNumbers ?? throw new Exception("Не свободного серийного номера: " + _serialNumber);
                    _serialNumber = "";
                    _barcode = "";
                }
                catch (Exception ex)
                {
                    _barcode = "";
                    _serialNumber = "";
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                _barcode = "";
                _serialNumber = "";
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CustomControlLibrary.MVVM;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using ModelModul.ViewModels;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace CashierWorkplaceModul.ViewModels
{
    class CashierWorkplaceViewModel : ViewModelBase
    {
        #region Properties

        private readonly IDialogService _dialogService;

        private int _errorPostCount = 0;
        private string _barcode;
        private string _serialNumber;

        private MovementGoods _salesGood = new MovementGoods();
        public MovementGoods SalesGood
        {
            get => _salesGood;
            set => SetProperty(ref _salesGood, value);
        }

        public ObservableCollection<MovementGoodsInfo> SalesGoodsList
        {
            get => _salesGood.MovementGoodsInfosCollection as ObservableCollection<MovementGoodsInfo>;
            set
            {
                _salesGood.MovementGoodsInfosCollection = value;
                RaisePropertyChanged("SalesGoodsList");
            }
        }

        private ObservableCollection<Store> _storesList = new ObservableCollection<Store>();
        public ObservableCollection<Store> StoresList
        {
            get => _storesList;
            set => SetProperty(ref _storesList, value);
        }

        public bool IsValidate
        {
            get
            {
                if (!string.IsNullOrEmpty(SalesGood.Error)) return false;
                if (SalesGood.IdDisposalStore == null || SalesGood.IdDisposalStore == 0) return false;
                if (SalesGoodsList.Count == 0) return false;
                if (SalesGoodsList.Any(p => p.Price <= 0 || p.Count <= 0)) return false;
                return !SalesGoodsList.Where(p => p.Product.KeepTrackSerialNumbers).Any(p =>
                    p.Product.SerialNumbersCollection.Any(s => string.IsNullOrEmpty(s.Value)));
            }
        }


        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PostCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardSerialNumbersCommand { get; }
        #endregion

        public CashierWorkplaceViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            NewMovementGood();
            PostCommand = new DelegateCommand(Post).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            ListenKeyboardSerialNumbersCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboardSerialNumbers);
        }

        #region PropertyChanged

        private void OnSalesGoodsListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (MovementGoodsInfo item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SalesInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (MovementGoodsInfo item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SalesInfosViewModelPropertyChanged;
                        if (item.Product.KeepTrackSerialNumbers)
                        {
                            item.Product.SerialNumbersCollection = new ObservableCollection<SerialNumber>();
                            ((ObservableCollection<SerialNumber>)item.Product.SerialNumbersCollection).CollectionChanged += OnSerialNumbersCollectionChanged;
                        }
                    }
                    break;
            }
            RaisePropertyChanged("SalesGoodsList");
            RaisePropertyChanged("IsValidate");
        }

        private void OnSerialNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SerialNumber item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SalesInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumber item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SalesInfosViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("IsValidate");
        }

        private void SalesInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
if (sender is MovementGoodsInfo movement && e.PropertyName == "Count")
            {
                if (movement.Product.KeepTrackSerialNumbers)
                {
                    int count = (int)movement.Count - movement.Product.SerialNumbersCollection.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            SerialNumberViewModel temp = new SerialNumberViewModel
                            {
                                IdProduct = movement.Product.Id,
                                Product = movement.Product,
                                IdStore = SalesGood.IdDisposalStore
                            };
                            temp.Product.SerialNumbersCollection = movement.Product.SerialNumbersCollection;
                            movement.Product.SerialNumbersCollection.Add(temp);
                        }
                    }
                    else if (count < 0)
                    {
                        for (int i = 0; i > count; i--)
                        {
                            var temp = movement.Product.SerialNumbersCollection.FirstOrDefault(s =>
                                string.IsNullOrEmpty(s.Value));
                            movement.Product.SerialNumbersCollection.Remove(temp ?? movement.Product.SerialNumbersCollection.Last());
                        }
                    }
                }
            }
            RaisePropertyChanged("SalesGoodsList");
            RaisePropertyChanged("IsValidate");
        }

        #endregion

        private async void Load()
        {
            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();

                StoresList = new ObservableCollection<Store>(await storeRepository.GetListAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void NewMovementGood()
        {
            Load();
            SalesGood = new MovementGoods { MovementGoodsInfosCollection = new ObservableCollection<MovementGoodsInfo>() };
            SalesGood.PropertyChanged += (o, e) => RaisePropertyChanged("IsValidate");

            IRepository<MovmentGoodType> movmentGoodTypeRepository = new SqlMovmentGoodTypeRepository();
            _salesGood.MovmentGoodType = await movmentGoodTypeRepository.GetItemAsync(MovmentGoodTypeSpecification.GetMovmentGoodTypeByCode("sale"));
            _salesGood.IdType = _salesGood.MovmentGoodType.Id;

            SalesGoodsList.CollectionChanged += OnSalesGoodsListCollectionChanged;
            RaisePropertyChanged("SalesGoodsList");
            RaisePropertyChanged("IsValidate");
        }

        #region DelegateCommand

        private async void Post()
        {
            if (_errorPostCount == 0 && MessageBox.Show(
                    "Вы уверены, что хотите провести продажу? Этот отчет невозможно будет изменить после.",
                    "Проведение продажи", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlSerialNumberRepository serialNumberRepository = new SqlSerialNumberRepository();
                MovementGoods temp = (MovementGoods) SalesGood.Clone();
                temp.DateClose = null;
                temp.DateCreate = null;
                foreach (var movementGoodsInfo in temp.MovementGoodsInfosCollection)
                {
                    foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
                    {
                        List<long> freeSerialNumbers =
                            await serialNumberRepository.GetFreeSerialNumbers(movementGoodsInfo.Product.Id,
                                serialNumber.Value, temp.IdDisposalStore.Value);
                        temp.SerialNumberLogsCollection.Add(new SerialNumberLog {IdSerialNumber = freeSerialNumbers.First(fs => temp.SerialNumberLogsCollection.All(s => s.IdSerialNumber != fs)) });
                    }

                    movementGoodsInfo.Product = null;
                }

                IRepository<MovementGoods> movementGoodsRepository = new SqlMovementGoodsRepository();
                await movementGoodsRepository.CreateAsync(temp);
                MessageBox.Show("Отчет о продаже добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _errorPostCount = 0;
                NewMovementGood();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sex && (sex.State == 2 || sex.State == 3))
                {
                    if (_errorPostCount == 5)
                    {
                        _errorPostCount = 0;
                        MessageBox.Show("Произошла ошибка проверьте данные", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    foreach (var movementGoodsInfo in SalesGood.MovementGoodsInfosCollection)
                    {
                        movementGoodsInfo.Count = movementGoodsInfo.Count;
                        foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
                        {
                            serialNumber.Value = serialNumber.Value;
                        }
                        foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
                        {
                            serialNumber.Id = 0;
                        }
                    }
                    RaisePropertyChanged("IsValidate");
                    if (IsValidate)
                    {
                        _errorPostCount++;
                        Post();
                    }
                    else
                    {
                        _errorPostCount = 0;
                        MessageBox.Show("Произошла ошибка проверьте данные", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    return;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            _dialogService.ShowDialog("Catalog", new DialogParameters(), Callback);
        }

        private void Callback(IDialogResult dialogResult)
        {
            _barcode = "";
            Product temp = dialogResult.Parameters.GetValue<Product>("product");
            if (temp != null) InsertProduct(temp);
        }

        private void DeleteProduct(Collection<object> obj)
        {
            _barcode = "";
            List<MovementGoodsInfo> list = obj.Cast<MovementGoodsInfo>().ToList();
            list.ForEach(item => SalesGoodsList.Remove(item));
        }

        private void ListenKeyboard(KeyEventArgs obj)
        {
            //if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
            //{
            //    _barcode += obj.Key.ToString()[1].ToString();
            //}
            //else if (obj.Key >= Key.A && obj.Key <= Key.Z)
            //{
            //    _barcode += obj.Key.ToString();
            //}
            //else if (obj.Key == Key.Enter)
            //{
            //    try
            //    {
            //        SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
            //        ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_barcode);

            //        SerialNumber freeSerialNumbers = null;
            //        foreach (var objSer in serialNumberses)
            //        {
            //            if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
            //            freeSerialNumbers = objSer;
            //            break;
            //        }

            //        SalesInfosViewModel saleInfos = SalesInfos.FirstOrDefault(objSale =>
            //            objSale.Product.WarrantyPeriod.Period != "Нет" && objSale.SerialNumber.Id == 0);

            //        if (freeSerialNumbers == null && saleInfos != null)
            //            throw new Exception("Сначала нужно указать серийный номер к товару: \"" +
            //                                saleInfos.Product.Title +
            //                                "\".\nЕсли это был серийный номер попробуйте указать его еще раз или используйте другой серийный номер, или ввести вручную");
            //        if (freeSerialNumbers != null && saleInfos != null)
            //        {
            //            saleInfos.SerialNumber = freeSerialNumbers;
            //            _barcode = "";
            //            return;
            //        }
            //        SqlProductRepository dbSetProducts = new SqlProductRepository();
            //        Product product = null;
            //        if (freeSerialNumbers != null)
            //        {
            //            product = dbSetProducts.FindProductById(freeSerialNumbers.IdProduct);
            //            if (product == null) throw new Exception("Товар не найден");
            //            SalesInfos.AddAsync(new SalesInfosViewModel
            //            {
            //                IdProduct = product.Id,
            //                Product = product,
            //                SellingPrice = product.SalesPrice,
            //                Count = 1,
            //                SerialNumber = freeSerialNumbers
            //            });
            //        }

            //        if (freeSerialNumbers == null)
            //        {
            //            if (serialNumberses.Count != 0) throw new Exception("Нет свободного серийного номера: " + _barcode);
            //            product = dbSetProducts.FindProductByBarcode(_barcode);
            //            if (product == null) throw new Exception("Товар не найден");
            //            saleInfos = SalesInfos.FirstOrDefault(objSale => objSale.IdProduct == product.Id);
            //            SalesInfosViewModel temp = new SalesInfosViewModel
            //            {
            //                IdProduct = product.Id,
            //                Product = product,
            //                SellingPrice = product.SalesPrice,
            //                Count = 1,
            //                SerialNumber = new SerialNumber()
            //            };
            //            if (saleInfos == null)
            //            {
            //                SalesInfos.AddAsync(temp);
            //            }
            //            else
            //            {
            //                if (product.WarrantyPeriod.Period == "Нет") saleInfos.Count++;
            //                else SalesInfos.AddAsync(temp);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _barcode = "";
            //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    _barcode = "";
            //}
            //else
            //{
            //    _barcode = "";
            //}
        }

        private void ListenKeyboardSerialNumbers(KeyEventArgs obj)
        {
            //if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
            //{
            //    _serialNumber += obj.Key.ToString()[1].ToString();
            //}
            //else if (obj.Key >= Key.A && obj.Key <= Key.Z)
            //{
            //    _serialNumber += obj.Key.ToString();
            //}
            //else if (obj.Key == Key.Enter)
            //{
            //    try
            //    {
            //        SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == 0 && objSale.Product.WarrantyPeriod.Period != "Нет");
            //        if (sale == null) throw new Exception("Возникла непредвиденная ошибка");
            //        SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
            //        ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_serialNumber, sale.IdProduct);
            //        if (serialNumberses.Count == 0) throw new Exception("Cерийный номер: \"" + _serialNumber + "\" для этого товара не найден: ");
            //        SerialNumber freeSerialNumbers = null;
            //        foreach (var objSer in serialNumberses)
            //        {
            //            if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
            //            freeSerialNumbers = objSer;
            //            break;
            //        }
            //        sale.SerialNumber = freeSerialNumbers ?? throw new Exception("Не свободного серийного номера: " + _serialNumber);
            //        _serialNumber = "";
            //        _barcode = "";
            //    }
            //    catch (Exception ex)
            //    {
            //        _barcode = "";
            //        _serialNumber = "";
            //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
            //else
            //{
            //    _barcode = "";
            //    _serialNumber = "";
            //}
        }

        private void InsertProduct(Product product)
        {
            try
            {
                if (SalesGoodsList.Any(objRev => objRev.Product.Id == product.Id))
                {
                    _barcode = "";
                    return;
                }

                SqlProductRepository productRepository = new SqlProductRepository();

                var currentPriceLoad = Task.Run(() => productRepository.GetCurrentPrice(product.Id));

                Task.WaitAll(currentPriceLoad);
                
                SalesGoodsList.Add(new MovementGoodsInfoViewModel { IdProduct = product.Id, Price = currentPriceLoad.Result, MovementGoods = SalesGood, Product = product, Count = 0, });
                RaisePropertyChanged("SalesGoodsList");
                RaisePropertyChanged("IsValidate");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

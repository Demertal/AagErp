using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace PurchaseGoodModul.ViewModels
{
    public class PurchaseGoodViewModel : ViewModelBase
    {
        #region Properties

        private string _barcode;

        private readonly IRegionManager _regionManager;

        private readonly IDialogService _dialogService;

        private MovementGoods _purchaseGood = new MovementGoods();
        public MovementGoods PurchaseGood
        {
            get => _purchaseGood;
            set => SetProperty(ref _purchaseGood, value);
        }

        public ObservableCollection<MovementGoodsInfo> PurchaseGoodsList
        {
            get => _purchaseGood.MovementGoodsInfosCollection as ObservableCollection<MovementGoodsInfo>;
            set
            {
                _purchaseGood.MovementGoodsInfosCollection = value;
                RaisePropertyChanged("PurchaseGoodsList");
            }
        }

        private ObservableCollection<Store> _storesList = new ObservableCollection<Store>();
        public ObservableCollection<Store> StoresList
        {
            get => _storesList;
            set => SetProperty(ref _storesList, value);
        }

        private ObservableCollection<Counterparty> _counterpartiesList = new ObservableCollection<Counterparty>();
        public ObservableCollection<Counterparty> CounterpartiesList
        {
            get => _counterpartiesList;
            set => SetProperty(ref _counterpartiesList, value);
        }

        private ObservableCollection<Currency> _currenciesList = new ObservableCollection<Currency>();
        public ObservableCollection<Currency> CurrenciesList
        {
            get => _currenciesList;
            set => SetProperty(ref _currenciesList, value);
        }

        private ObservableCollection<Currency> _equivalentCurrenciesList = new ObservableCollection<Currency>();
        public ObservableCollection<Currency> EquivalentCurrenciesList
        {
            get => _equivalentCurrenciesList;
            set => SetProperty(ref _equivalentCurrenciesList, value);
        }

        public bool IsValidate
        {
            get
            {
                if(!string.IsNullOrEmpty(PurchaseGood.Error)) return false;
                if (PurchaseGood.EquivalentRate == null || PurchaseGood.EquivalentRate <= 0 ||
                    PurchaseGood.Rate == null || PurchaseGood.Rate <= 0 || PurchaseGood.IdEquivalentCurrency == null ||
                    PurchaseGood.IdEquivalentCurrency == 0 || PurchaseGood.IdCurrency == null ||
                    PurchaseGood.IdCurrency == 0 || PurchaseGood.IdArrivalStore == null ||
                    PurchaseGood.IdArrivalStore == 0 || PurchaseGood.IdCounterparty == null ||
                    PurchaseGood.IdCounterparty == 0) return false;
                if (PurchaseGoodsList.Count == 0) return false;
                if (PurchaseGoodsList.Any(p => p.Price <= 0 || p.Count <= 0)) return false;
                return !PurchaseGoodsList.Where(p => p.Product.KeepTrackSerialNumbers).Any(p =>
                    p.Product.SerialNumbers.Any(s => string.IsNullOrEmpty(s.Value)));
            }
        }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PostCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }

        #endregion

        public PurchaseGoodViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            PostCommand = new DelegateCommand(Post).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            NewMovementGood();
        }

        #region PropertyChanged

        private void OnPurchaseGoodsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (MovementGoodsInfo item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (MovementGoodsInfo item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += PurchaseInfosViewModelPropertyChanged;
                        if (item.Product.KeepTrackSerialNumbers)
                        {
                            item.Product.SerialNumbers = new ObservableCollection<SerialNumber>();
                            ((ObservableCollection<SerialNumber>)item.Product.SerialNumbers).CollectionChanged += OnSerialNumbersCollectionChanged;
                        }
                    }
                    break;
            }
            RaisePropertyChanged("PurchaseGoodsList");
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
                        item.PropertyChanged -= PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumber item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += PurchaseInfosViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("IsValidate");
        }

        private void PurchaseInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is MovementGoodsInfo movement && e.PropertyName == "Count")
            {
                if(movement.Product.KeepTrackSerialNumbers)
                {
                    int count = (int) movement.Count - movement.Product.SerialNumbers.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                            movement.Product.SerialNumbers.Add(new SerialNumber());
                    }
                    else if (count < 0)
                    {
                        for (int i = 0; i > count; i--)
                        {
                            var temp = movement.Product.SerialNumbers.FirstOrDefault(s =>
                                string.IsNullOrEmpty(s.Value));
                            movement.Product.SerialNumbers.Remove(temp ?? movement.Product.SerialNumbers.Last());
                        }
                    }
                }
            }
            RaisePropertyChanged("PurchaseGoodsList");
            RaisePropertyChanged("IsValidate");
        }

        #endregion

        private void Load()
        {
            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();

                var loadStore = Task.Run(() => storeRepository.GetListAsync());
                var loadCurrency = Task.Run(() => currencyRepository.GetListAsync());
                var loadCounterparty = Task.Run(() => counterpartyRepository.GetListAsync(CounterpartySpecification.GetCounterpartiesByType(TypeCounterparties.Suppliers)));

                Task.WaitAll(loadStore, loadCurrency, loadCounterparty);

                StoresList = new ObservableCollection<Store>(loadStore.Result);
                CurrenciesList = new ObservableCollection<Currency>(loadCurrency.Result);
                EquivalentCurrenciesList = new ObservableCollection<Currency>(loadCurrency.Result);
                CounterpartiesList = new ObservableCollection<Counterparty>(loadCounterparty.Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void NewMovementGood()
        {
            Load();
            PurchaseGood = new MovementGoods { MovementGoodsInfosCollection = new ObservableCollection<MovementGoodsInfo>() };
            PurchaseGood.PropertyChanged += (o, e) => RaisePropertyChanged("IsValidate");

            IRepository<MovmentGoodType> movmentGoodTypeRepository = new SqlMovmentGoodTypeRepository();
            _purchaseGood.MovmentGoodType = await movmentGoodTypeRepository.GetItemAsync(MovmentGoodTypeSpecification.GetMovmentGoodTypeByCode("purchase"));
            _purchaseGood.IdType = _purchaseGood.MovmentGoodType.Id;

            PurchaseGoodsList.CollectionChanged += OnPurchaseGoodsCollectionChanged;
            RaisePropertyChanged("PurchaseGoodsList");
            RaisePropertyChanged("IsValidate");
        }

        private void Navigate(List<Product> revaluationProducts)
        {
            //NavigationParameters navigationParameters =
            //    new NavigationParameters { { "RevaluationProducts", revaluationProducts } };
            //_regionManager.RequestNavigate("ContentRegion", "RevaluationProductsView", navigationParameters);
        }

        #region DelegateCommand

        private async void Post()
        {
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о закупке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                MovementGoods temp = (MovementGoods)PurchaseGood.Clone();
                temp.DateClose = null;
                temp.DateCreate = null;
                foreach (var movementGoodsInfo in temp.MovementGoodsInfosCollection)
                {
                    movementGoodsInfo.EquivalentCost = movementGoodsInfo.Price / temp.EquivalentRate;
                    foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbers)
                    {
                        serialNumber.DateCreated = null;
                        serialNumber.IdProduct = movementGoodsInfo.Product.Id;
                        temp.SerialNumberLogsCollection.Add(new SerialNumberLog{SerialNumber = serialNumber });
                    }
                    movementGoodsInfo.Product = null;
                }

                IRepository<MovementGoods> movementGoodsRepository = new SqlMovementGoodsRepository();
                await movementGoodsRepository.CreateAsync(temp);
                MessageBox.Show("Отчет о закупке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NewMovementGood();

                //_report.PurchaseInfos = new List<PurchaseInfos>();
                //foreach (var purchaseInfo in PurchaseInfos)
                //{
                //    _report.PurchaseInfos.AddAsync(purchaseInfo.PurchaseInfo);
                //}
                //SqlMovementGoodsRepository dbSet = new SqlMovementGoodsRepository();
                //dbSet.AddAsync((PurchaseReports)_report.Clone());
                //MessageBox.Show("Отчет о закупке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                //if (PurchaseInfos.FirstOrDefault(objPr =>
                //        objPr.Product.SalesPrice == 0 ||
                //        Math.Abs(objPr.PurchasePrice - objPr.PurchasePriceOld) > (decimal)0.01 ||
                //        objPr.Currency.Id != objPr.ExchangeRateOld.Id) != null)
                //{
                //    if (MessageBox.Show(
                //            "Есть товар на который изменилась цена закупки. Вы хотите переоценить этот товар?",
                //            "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                //        MessageBoxResult.Yes)
                //    {
                //        NewMovementGood();
                //        return;
                //    }
                //    List<Product> revaluationProducts = PurchaseInfos
                //        .Where(objPr =>
                //            Math.Abs(objPr.PurchasePrice - objPr.PurchasePriceOld) > (decimal)0.01 ||
                //            objPr.Currency.Id != objPr.ExchangeRateOld.Id).Select(objPr => objPr.Product).ToList();
                //    Navigate(revaluationProducts);
                //}
                //NewMovementGood();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            _dialogService.ShowDialog("ShowProduct", new DialogParameters(), Callback);
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
            list.ForEach(item => PurchaseGoodsList.Remove(item));
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
            //        if (string.IsNullOrEmpty(_barcode) || _barcode.Length < 8 || _barcode.Length > 13)
            //        {
            //            _barcode = "";
            //            return;
            //        }
            //        SqlProductRepository dbSetProducts = new SqlProductRepository();
            //        Product product = dbSetProducts.FindProductByBarcode(_barcode);
            //        if (product == null) throw new Exception("Товар не найден");
            //        AddProduct(product);
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

        private async void InsertProduct(Product product)
        {
            try
            {
                if (PurchaseGoodsList.Any(objRev => objRev.Product.Id == product.Id))
                {
                    _barcode = "";
                    return;
                }

                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                product.UnitStorage = await unitStorageRepository.GetItemAsync(product.IdUnitStorage);

                PurchaseGoodsList.Add(new MovementGoodsInfo{Count = 0, IdProduct = product.Id, Price = 0, Product = product});
                RaisePropertyChanged("PurchaseGoodsList");
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ModelModul;
using ModelModul.ExchangeRate;
using ModelModul.Product;
using ModelModul.RevaluationProduct;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using RevaluationProductsModul.Views;

namespace RevaluationProductsModul.ViewModels
{
    public class RevaluationProductsViewModel : ViewModelBase
    {
        #region Properties

        private string _barcode;

        private RevaluationProductsReports _revaluationProductsReports = new RevaluationProductsReports();

        private ObservableCollection<RevaluationProductsForRevaluationViewModel> _revaluationProductsInfosInfos = new ObservableCollection<RevaluationProductsForRevaluationViewModel>();
        public ObservableCollection<RevaluationProductsForRevaluationViewModel> RevaluationProductsInfos
        {
            get => _revaluationProductsInfosInfos;
            set => SetProperty(ref _revaluationProductsInfosInfos, value);
        }

        public bool IsValidate
        {
            get
            {
                if (RevaluationProductsInfos.Count == 0) return false;

                return RevaluationProductsInfos.FirstOrDefault(rev => rev.NewSalesPrice <= 0) == null;
            }
        }

        public InteractionRequest<Confirmation> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand RevaluationCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }

        public DelegateCommand<object> TestCommand { get; }

        #endregion

        public RevaluationProductsViewModel()
        {
            AddProductPopupRequest = new InteractionRequest<Confirmation>();
            RevaluationCommand = new DelegateCommand(Revaluation).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            RevaluationProductsInfos.CollectionChanged += OnRevaluationProductsCollectionChanged;
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            TestCommand = new DelegateCommand<object>(Test);
            NewReport();
        }

        private void Test(object obj)
        {
            (((RoutedEventArgs) obj).OriginalSource as RevaluationProductsView).BringPurchasePriceCh.Focus();
        }

        #region PropertyChanged

        private void OnRevaluationProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (RevaluationProductsForRevaluationViewModel item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (RevaluationProductsForRevaluationViewModel item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
            }

            _barcode = "";
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationProductsView");
        }

        private void RevaluationProductsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("RevaluationProductsView");
            RaisePropertyChanged("IsValidate");
        }

        #endregion

        private void NewReport()
        {
            _barcode = "";
            _revaluationProductsReports = new RevaluationProductsReports();
            RevaluationProductsInfos.Clear();
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationProductsView");
        }

        #region DelegateCommand

        private void Revaluation()
        {
            _barcode = "";
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о переоценке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                bool ch = false;
                foreach (var revaluationProductsInfo in RevaluationProductsInfos)
                {
                    _revaluationProductsReports.RevaluationProductsInfos.Add(revaluationProductsInfo.RevaluationProductsInfo);
                    if (revaluationProductsInfo.NewSalesPrice <= revaluationProductsInfo.PurchasePrice) ch = true;
                }

                if (ch)
                {
                    if (MessageBox.Show(
                            "На некоторый товар цена продажи меньше цены закупки. Все верно?",
                            "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                }

                DbSetRevaluationProducts dbSet = new DbSetRevaluationProducts();
                dbSet.Add(_revaluationProductsReports);
                MessageBox.Show("Отчет о переоценке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            _barcode = "";
            AddProductPopupRequest.Raise(new Confirmation { Title = "Выбрать товар" }, Callback);
        }

        private void DeleteProduct(Collection<object> obj)
        {
            _barcode = "";
            List<RevaluationProductsForRevaluationViewModel> list = obj.Cast<RevaluationProductsForRevaluationViewModel>().ToList();
            list.ForEach(item => RevaluationProductsInfos.Remove(item));
            list.ForEach(item => _revaluationProductsReports.RevaluationProductsInfos.Remove(item.RevaluationProductsInfo));
        }

        private void Callback(INotification obj)
        {
            _barcode = "";
            if (!(obj.Content is Products)) return;
            InsertProduct((Products) obj.Content);
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
                    if (string.IsNullOrEmpty(_barcode) || _barcode.Length < 8 || _barcode.Length > 13)
                    {
                        _barcode = "";
                        return;
                    }
                    DbSetProducts dbSetProducts = new DbSetProducts();
                    Products product = dbSetProducts.FindProductByBarcode(_barcode);
                    if (product == null) throw new Exception("Товар не найден");
                    InsertProduct(product);
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

        private void InsertProduct(Products product)
        {
            try
            {
                if (RevaluationProductsInfos.FirstOrDefault(objRev => objRev.Product.Id == product.Id) != null)
                {
                    _barcode = "";
                    return;
                }
                DbSetProducts dbSetProducts = new DbSetProducts();
                PurchaseStruct purchaseStructCur = dbSetProducts.GetPurchasePrice(product.Id);
                PurchaseStruct purchaseStructLast = null;
                DbSetExchangeRates dbSetExchange = new DbSetExchangeRates();
                ExchangeRates exchange = dbSetExchange.Load("ГРН");
                if (purchaseStructCur != null)
                {
                    purchaseStructLast = dbSetProducts.GetPurchasePrice(product.Id, 1);
                }
                RevaluationProductsInfos.Add(new RevaluationProductsForRevaluationViewModel
                {
                    RevaluationProductsInfo = new RevaluationProductsInfos
                    {
                        Products = product,
                        IdProduct = product.Id,
                        OldSalesPrice = product.SalesPrice,
                        NewSalesPrice = 0
                    },
                    ExchangeRate = purchaseStructCur?.ExchangeRate ?? exchange,
                    PurchasePrice = purchaseStructCur?.PurchasePrice ?? 0,
                    ExchangeRateOld = purchaseStructLast?.ExchangeRate ?? exchange,
                    PurchasePriceOld = purchaseStructLast?.PurchasePrice ?? 0
                });

                RaisePropertyChanged("RevaluationProductsInfos");
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
            if (!(navigationContext.Parameters["RevaluationProducts"] is List<Products> revaluationProducts)) return;
            foreach (var product in revaluationProducts)
            {
                if (RevaluationProductsInfos.FirstOrDefault(objRev => objRev.Product.Id == product.Id) !=
                    null) continue;
                InsertProduct(product);
            }

            RaisePropertyChanged("RevaluationProductsInfos");
            RaisePropertyChanged("IsValidate");
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
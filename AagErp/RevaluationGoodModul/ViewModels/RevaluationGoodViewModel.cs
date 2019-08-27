using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using RevaluationProduct = ModelModul.Models.RevaluationProduct;

namespace RevaluationGoodModul.ViewModels
{
    public class RevaluationGoodViewModel : ViewModelBase
    {
        #region Properties

        private readonly IDialogService _dialogService;

        private string _barcode;

        private RevaluationProduct _revaluationProduct = new RevaluationProduct();
        public ObservableCollection<PriceProduct> RevaluationPriceProductsList
        {
            get => _revaluationProduct.PriceProducts as ObservableCollection<PriceProduct>;
            set
            {
                _revaluationProduct.PriceProducts = value;
                RaisePropertyChanged("RevaluationPriceProductsList");
            }
        }

        private ObservableCollection<Currency> _currenciesList;
        public ObservableCollection<Currency> CurrenciesList
        {
            get => _currenciesList;
            set
            {
                _currenciesList = value;
                RaisePropertyChanged("CurrenciesList");
            }
        }

        public bool IsValidate
        {
            get
            {
                if (RevaluationPriceProductsList.Count == 0) return false;

                return !RevaluationPriceProductsList.Any(p => p.Price <= 0);
            }
        }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PostCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }

        //public DelegateCommand<object> TestCommand { get; }

        #endregion

        public RevaluationGoodViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            NewRevaluationProduct();
            PostCommand = new DelegateCommand(Post).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            //TestCommand = new DelegateCommand<object>(Test);
            NewRevaluationProduct();
        }

        //private void Test(object obj)
        //{
        //    (((RoutedEventArgs)obj).OriginalSource as Views.RevaluationGood).BringPurchasePriceCh.Focus();
        //}

        #region PropertyChanged

        private void OnRevaluationPriceProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (PriceProduct item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (PriceProduct item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
            }

            _barcode = "";
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationPriceProductsList");
        }

        private void RevaluationProductsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("RevaluationPriceProductsList");
            RaisePropertyChanged("IsValidate");
        }

        #endregion

        private void NewRevaluationProduct()
        {
            _barcode = "";
            _revaluationProduct = new RevaluationProduct();
            RevaluationPriceProductsList = new ObservableCollection<PriceProduct>();
            RevaluationPriceProductsList.CollectionChanged += OnRevaluationPriceProductsCollectionChanged;
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationPriceProductsList");
        }

        #region DelegateCommand

        private async void Post()
        {
            _barcode = "";
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о переоценке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                RevaluationProduct temp = (RevaluationProduct)_revaluationProduct.Clone();
                temp.DateRevaluation = null;
                temp.PriceProducts.ToList().ForEach(pp => pp.Product = null);
                IRepository<RevaluationProduct> revaluationProductRepository = new SqlRevaluationProductRepository();
                await revaluationProductRepository.CreateAsync(temp);
                MessageBox.Show("Отчет о переоценке добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NewRevaluationProduct();
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
            List<PriceProduct> list = obj.Cast<PriceProduct>().ToList();
            list.ForEach(item => RevaluationPriceProductsList.Remove(item));
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
                    //SqlProductRepository dbSetProducts = new SqlProductRepository();
                    //Product product = dbSetProducts.FindProductByBarcode(_barcode);
                    //if (product == null) throw new Exception("Товар не найден");
                    //InsertProduct(product);
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

        private async void InsertProduct(Product product)
        {
            try
            {
                if (RevaluationPriceProductsList.Any(objRev => objRev.Product.Id == product.Id))
                {
                    _barcode = "";
                    return;
                }

                SqlProductRepository productRepository = new SqlProductRepository();
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                product.EquivalentCostForЕxistingProducts =
                    new ObservableCollection<EquivalentCostForЕxistingProduct>(
                        await productRepository.GetEquivalentCostsForЕxistingProduct(product.Id));
                product.Price = await productRepository.GetCurrentPrice(product.Id);
                product.PriceGroup = await priceGroupRepository.GetItemAsync(product.IdPriceGroup.Value);
                EquivalentCostForЕxistingProduct equivalentCost = product.EquivalentCostForЕxistingProducts
                    .OrderByDescending(c => c.EquivalentCost).FirstOrDefault();
                RevaluationPriceProductsList.Add(equivalentCost == null
                    ? new PriceProduct {IdProduct = product.Id, Product = product, Price = 0}
                    : new PriceProduct
                    {
                        IdProduct = product.Id,
                        Product = product,
                        Price = equivalentCost.EquivalentCost * (1 + product.PriceGroup.Markup) *
                                equivalentCost.EquivalentCurrency.Cost
                    });
                RaisePropertyChanged("RevaluationPriceProductsList");
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
            //if (!(navigationContext.Parameters["RevaluationGood"] is List<Product> revaluationProducts)) return;
            //foreach (var product in revaluationProducts)
            //{
            //    if (RevaluationProductsInfos.FirstOrDefault(objRev => objRev.Product.Id == product.Id) !=
            //        null) continue;
            //    InsertProduct(product);
            //}

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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModelModul.Models;
using ModelModul.MVVM;
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

        //private string _barcode;

        private RevaluationProduct _revaluationProduct = new RevaluationProduct();
        public ObservableCollection<PriceProduct> RevaluationPriceProductsList
        {
            get => _revaluationProduct.PriceProductsCollection as ObservableCollection<PriceProduct>;
            set
            {
                _revaluationProduct.PriceProductsCollection = value;
                RaisePropertyChanged("RevaluationPriceProductsList");
            }
        }

        private ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
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

        public bool IsValidate => RevaluationPriceProductsList.Count != 0 && RevaluationPriceProductsList.All(p => p.IsValidate);

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PostCommand { get; }
        public DelegateCommand<PriceProduct> DeleteProductCommand { get; }
        //public DelegateCommand<KeyEventArgs> ListenKeyboardCommand { get; }

        #endregion

        public RevaluationGoodViewModel(IDialogService dialogService) : base(dialogService)
        {
            NewRevaluationProduct();
            PostCommand = new DelegateCommand(Post).ObservesCanExecute(() => IsValidate);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<PriceProduct>(DeleteProduct);
            //ListenKeyboardCommand = new DelegateCommand<KeyEventArgs>(ListenKeyboard);
            NewRevaluationProduct();
        }

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
                        CalculatePrice(item);
                        item.PropertyChanged += RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
            }

            //_barcode = "";
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationPriceProductsList");
        }

        private void RevaluationProductsViewModelPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            CalculatePrice((PriceProduct) sender);
            RaisePropertyChanged("RevaluationPriceProductsList");
            RaisePropertyChanged("IsValidate");
        }

        #endregion

        private void NewRevaluationProduct()
        {
            //_barcode = "";
            _revaluationProduct = new RevaluationProduct();
            RevaluationPriceProductsList = new ObservableCollection<PriceProduct>();
            RevaluationPriceProductsList.CollectionChanged += OnRevaluationPriceProductsCollectionChanged;
            LoadAsync();
            RaisePropertyChanged("IsValidate");
            RaisePropertyChanged("RevaluationPriceProductsList");
        }

        private void CalculatePrice(PriceProduct priceProduct)
        {
            if (priceProduct.Product?.EquivalentCostForExistingProductsCollection != null &&
                priceProduct.Product.EquivalentCostForExistingProductsCollection.Count != 0 && CurrenciesList != null &&
                CurrenciesList.Count != 0 && priceProduct.Product.PriceGroup != null)
                priceProduct.Price = priceProduct.Product.EquivalentCostForExistingProductsCollection
                                 .OrderByDescending(e =>
                                     e.EquivalentCost * CurrenciesList.First(c => c.Id == e.EquivalentCurrencyId).Cost)
                                 .Select(e =>
                                     e.EquivalentCost * CurrenciesList.First(c => c.Id == e.EquivalentCurrencyId).Cost)
                                 .FirstOrDefault() * (1 + priceProduct.Product.PriceGroup.Markup);
            else priceProduct.Price = 0;
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync());
                var loadCurrency = Task.Run(() => currencyRepository.GetListAsync());

                Task.WaitAll(loadUnitStorage, loadCurrency);


                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                CurrenciesList = new ObservableCollection<Currency>(loadCurrency.Result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region DelegateCommand

        private async void Post()
        {
            //_barcode = "";
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о переоценке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                RevaluationProduct temp = (RevaluationProduct)_revaluationProduct.Clone();
                temp.DateRevaluation = null;
                temp.PriceProductsCollection.ToList().ForEach(pp => pp.Product = null);
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
            DialogService.ShowDialog("Catalog", new DialogParameters(), Callback);
        }

        private void Callback(IDialogResult dialogResult)
        {
            //_barcode = "";
            Product temp = dialogResult.Parameters.GetValue<Product>("product");
            if (temp != null) InsertProduct(temp);
        }

        private void DeleteProduct(PriceProduct obj)
        {
            //_barcode = "";
            RevaluationPriceProductsList.Remove(obj);
        }

        //private void ListenKeyboard(KeyEventArgs obj)
        //{
        //    if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
        //    {
        //        _barcode += obj.Key.ToString()[1].ToString();
        //    }
        //    else if (obj.Key >= Key.A && obj.Key <= Key.Z)
        //    {
        //        _barcode += obj.Key.ToString();
        //    }
        //    else if (obj.Key == Key.Enter)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(_barcode) || _barcode.Length < 8 || _barcode.Length > 13)
        //            {
        //                _barcode = "";
        //                return;
        //            }
        //            //SqlProductRepository dbSetProducts = new SqlProductRepository();
        //            //Product product = dbSetProducts.FindProductByBarcode(_barcode);
        //            //if (product == null) throw new Exception("Товар не найден");
        //            //InsertProduct(product);
        //        }
        //        catch (Exception ex)
        //        {
        //            _barcode = "";
        //            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        _barcode = "";
        //    }
        //    else
        //    {
        //        _barcode = "";
        //    }
        //}

        private void InsertProduct(Product product)
        {
            try
            {
                if (RevaluationPriceProductsList.Any(objRev => objRev.Product.Id == product.Id))
                {
                    //_barcode = "";
                    return;
                }

                SqlProductRepository productRepositoryPrice = new SqlProductRepository();
                SqlProductRepository productRepositoryEquivalentCost = new SqlProductRepository();
                
                var currentPriceLoad = Task.Run(() => productRepositoryPrice.GetCurrentPrice(product.Id));
                var equivalentCostLoad = Task.Run(() =>
                    productRepositoryEquivalentCost.GetEquivalentCostsForЕxistingProduct(product.Id));

                Task.WaitAll(currentPriceLoad, equivalentCostLoad);

                product.Price = currentPriceLoad.Result;
                product.EquivalentCostForExistingProductsCollection =
                    new ObservableCollection<EquivalentCostForExistingProduct>(equivalentCostLoad.Result);
                product.Count = product.EquivalentCostForExistingProductsCollection.Sum(e => e.Count);

                RevaluationPriceProductsList.Add(new PriceProduct {IdProduct = product.Id, Product = product});
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
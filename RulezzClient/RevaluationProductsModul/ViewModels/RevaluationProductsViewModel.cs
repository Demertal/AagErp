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
using ModelModul.RevaluationProduct;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace RevaluationProductsModul.ViewModels
{
    public class RevaluationProductsViewModel : BindableBase
    {
        #region Properties

        private readonly DbSetExchangeRates _dbSetExchangeRates = new DbSetExchangeRates();

        private ObservableCollection<RevaluationProducts> _revaluationProducts = new ObservableCollection<RevaluationProducts>();

        public ObservableCollection<RevaluationProducts> RevaluationProducts
        {
            get => _revaluationProducts;
            set
            {
                _revaluationProducts = value;
                RaisePropertyChanged();
            }
        }

        ExchangeRates _exchangeUsd = new ExchangeRates();

        public decimal Course => _exchangeUsd.Course;

        public bool IsEnabled
        {
            get
            {
                if (RevaluationProducts.Count == 0) return false;

                return RevaluationProducts.Count(rev => rev.NewSalesPrice <= 0) == 0;
            }
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand RevaluationCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }

        #endregion

        public RevaluationProductsViewModel()
        {
            AddProductPopupRequest = new InteractionRequest<INotification>();
            RevaluationCommand = new DelegateCommand(Revaluation).ObservesCanExecute(() => IsEnabled);
            AddProductCommand = new DelegateCommand(AddProduct);
            RevaluationProducts.CollectionChanged += OnRevaluationProductsCollectionChanged;
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            NewReport();
        }

        #region PropertyChanged

        private void OnRevaluationProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (RevaluationProducts item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (RevaluationProducts item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += RevaluationProductsViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("IsEnabled");
        }

        private void RevaluationProductsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("RevaluationProductsView");
            RaisePropertyChanged("IsEnabled");
        }

        #endregion

        private async void Load()
        {
            try
            {
                await _dbSetExchangeRates.LoadAsync();
                RaisePropertyChanged("ExchangeRates");
                _exchangeUsd = _dbSetExchangeRates.List.FirstOrDefault(e => e.Title == "USD");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewReport()
        {
            Load();
            RevaluationProducts.Clear();
            RaisePropertyChanged("IsEnabled");
            RaisePropertyChanged("RevaluationProductsView");
        }

        #region DelegateCommand

        private async void Revaluation()
        {
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести отчет о переоценке товара? Этот отчет невозможно будет изменить после.",
                    "Проведение закупки", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetRevaluationProducts dbSetRevaluation = new DbSetRevaluationProducts();
                await dbSetRevaluation.AddAsync(RevaluationProducts.ToList());
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
            AddProductPopupRequest.Raise(new Confirmation { Title = "Выбрать товар" }, Callback);
        }

        private void DeleteProduct(Collection<object> obj)
        {
            List<RevaluationProducts> list = obj.Cast<RevaluationProducts>().ToList();
            list.ForEach(item => RevaluationProducts.Remove(item));
            RaisePropertyChanged("RevaluationProducts");
        }

        private void Callback(INotification obj)
        {
            if (obj.Content == null) return;
            foreach (Products product in (IEnumerable)obj.Content)
            {
                if (RevaluationProducts.Count(p => p.Products.Id == product.Id) != 0) continue;
                RevaluationProducts.Add(new RevaluationProducts
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
                        Title = product.Title
                    },
                    IdProduct = product.Id,
                    OldSalesPrice = product.SalesPrice
                });
            }
            RaisePropertyChanged("RevaluationProducts");
        }

        #endregion
    }
}
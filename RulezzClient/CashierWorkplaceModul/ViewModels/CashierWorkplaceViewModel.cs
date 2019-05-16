using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace CashierWorkplaceModul.ViewModels
{
    class CashierWorkplaceViewModel: BindableBase
    {
        #region Properties

        private readonly SalesReports _salesReports = new SalesReports();

        public ObservableCollection<SalesInfos> SalesInfos =>
            _salesReports.SalesInfos as ObservableCollection<SalesInfos>;

        public bool IsEnabled
        {
            get
            {
                if (SalesInfos.Count == 0) return false;

                return SalesInfos.Count(sal => sal.Count <= 0) == 0;
            }
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand SaleCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> UpdatSalesInfosCommand { get; }

        #endregion

        public CashierWorkplaceViewModel()
        {
            AddProductPopupRequest = new InteractionRequest<INotification>();
            SaleCommand = new DelegateCommand(Sale).ObservesCanExecute(() => IsEnabled);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
            _salesReports.SalesInfos = new ObservableCollection<SalesInfos>();
            SalesInfos.CollectionChanged += OnSalesInfosCollectionChanged;
            UpdatSalesInfosCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(UpdatSalesInfos);
            NewReport();
        }

        private void UpdatSalesInfos(DataGridCellEditEndingEventArgs obj)
        {
            if (obj.Column.Header == "Штрихкод")
            {
                //obj.Row.
            }
        }

        #region PropertyChanged

        private void OnSalesInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SalesInfos item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SalesInfosViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SalesInfos item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SalesInfosViewModelPropertyChanged;
                    }
                    break;
            }
            RaisePropertyChanged("IsEnabled");
        }

        private void SalesInfosViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SalesInfos");
            RaisePropertyChanged("IsEnabled");
        }

        #endregion

        private void Load()
        {
            //try
            //{
            //    await _dbSetExchangeRates.LoadAsync();
            //    RaisePropertyChanged("ExchangeRates");
            //    _exchangeUsd = _dbSetExchangeRates.List.FirstOrDefault(e => e.Title == "USD");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void NewReport()
        {
            //LoadAsync();
            SalesInfos.Clear();
            RaisePropertyChanged("IsEnabled");
            RaisePropertyChanged("SalesInfos");
        }

        #region DelegateCommand

        private void Sale()
        {
            if (MessageBox.Show(
                    "Вы уверены, что хотите провести продажу? Этот отчет невозможно будет изменить после.",
                    "Проведение продажи", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                //DbSetRevaluationProducts dbSetRevaluation = new DbSetRevaluationProducts();
                //dbSetRevaluation.AddAsync(SalesInfos.ToList());
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
            List<SalesInfos> list = obj.Cast<SalesInfos>().ToList();
            list.ForEach(item => SalesInfos.Remove(item));
            RaisePropertyChanged("SalesInfos");
        }

        private void Callback(INotification obj)
        {
            if (obj.Content == null) return;
            foreach (Products product in (IEnumerable)obj.Content)
            {
                if (SalesInfos.Count(p => p.Products.Id == product.Id) != 0) continue;
                SalesInfos.Add(new SalesInfos
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
                    SellingPrice = product.SalesPrice
                });
            }
            RaisePropertyChanged("SalesInfos");
        }

        #endregion
    }
}

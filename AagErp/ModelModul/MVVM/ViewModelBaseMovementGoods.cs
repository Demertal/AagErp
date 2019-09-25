using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class ViewModelBaseMovementGoods<TMovementGoodInfo> : ViewModelBase
        where TMovementGoodInfo : MovementGoodsInfo, new()
    {
        #region Properties

        private readonly string _movementGoodTypeCode, _askPost;

        protected CancellationTokenSource CancelTokenLoad;
        protected CancellationTokenSource CancelTokenNewReport;

        private MovementGoods _movementGoodsReport = new MovementGoods();
        public MovementGoods MovementGoodsReport
        {
            get => _movementGoodsReport;
            set
            {
                if(_movementGoodsReport != null)
                    _movementGoodsReport.PropertyChanged -= MovementGoodsReportOnPropertyChanged;
                SetProperty(ref _movementGoodsReport, value);
                if (_movementGoodsReport != null)
                    _movementGoodsReport.PropertyChanged += MovementGoodsReportOnPropertyChanged;
            }
        }

        public ObservableCollection<MovementGoodsInfo> MovementGoodsInfosList
        {
            get => MovementGoodsReport.MovementGoodsInfosCollection as ObservableCollection<MovementGoodsInfo>;
            set
            {
                MovementGoodsReport.MovementGoodsInfosCollection = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Store> _arrivalStoresList = new ObservableCollection<Store>();
        public ObservableCollection<Store> ArrivalStoresList
        {
            get => _arrivalStoresList;
            set => SetProperty(ref _arrivalStoresList, value);
        }

        private ObservableCollection<Store> _disposalStoresList = new ObservableCollection<Store>();
        public ObservableCollection<Store> DisposalStoresList
        {
            get => _disposalStoresList;
            set => SetProperty(ref _disposalStoresList, value);
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

        #endregion

        #region Command

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand PostCommand { get; }
        public DelegateCommand<Collection<object>> DeleteProductCommand { get; }

        #endregion

        protected ViewModelBaseMovementGoods(IDialogService dialogService, string movementGoodTypeCode, string askPost) : base(dialogService)
        {
            _movementGoodTypeCode = movementGoodTypeCode;
            _askPost = askPost;
            NewReport();
            PostCommand = new DelegateCommand(Post).ObservesCanExecute(() => MovementGoodsReport.IsValid);
            AddProductCommand = new DelegateCommand(AddProduct);
            DeleteProductCommand = new DelegateCommand<Collection<object>>(DeleteProduct);
        }

        #region PropertyChanged

        protected abstract void MovementGoodsReportOnPropertyChanged(object sender, PropertyChangedEventArgs ea);

        #endregion

        private async void LoadAsync()
        {
            CancelTokenLoad?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenLoad = newCts;

            try
            {
                await InLoad();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenLoad == newCts)
                CancelTokenLoad = null;
        }

        protected abstract Task InLoad();

        private async void NewReport()
        {
            MovementGoodsReport = new MovementGoods();
            MovementGoodsInfosList = new ObservableCollection<MovementGoodsInfo>();

            LoadAsync();

            CancelTokenNewReport?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenNewReport = newCts;

            try
            {
                IRepository<MovmentGoodType> movmentGoodTypeRepository = new SqlMovmentGoodTypeRepository();
                MovementGoodsReport.MovmentGoodType = await movmentGoodTypeRepository.GetItemAsync(CancelTokenNewReport.Token, MovmentGoodTypeSpecification.GetMovmentGoodTypeByCode(_movementGoodTypeCode));
                MovementGoodsReport.IdType = MovementGoodsReport.MovmentGoodType.Id;
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenNewReport == newCts)
                CancelTokenNewReport = null;
        }

        private async void ReloadAsync()
        {
            LoadAsync();

            if (MovementGoodsInfosList.Count == 0) return;

            CancelTokenLoad?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenLoad = newCts;

            try
            {
                SqlProductRepository productRepository = new SqlProductRepository();

                long[] idArray = MovementGoodsInfosList.Select(r => r.Product.Id).ToArray();

                List<Product> products = (await productRepository.GetProductsWithCountAndPrice(CancelTokenLoad.Token,
                        ProductSpecification.GetProductsById(idArray))).ToList();

                MovementGoodsInfosList = new ObservableCollection<MovementGoodsInfo>();

                foreach (var product in products)
                {
                    MovementGoodsInfosList.Add(await CreateMovementGoodInfoAsync(product));
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenLoad == newCts)
                CancelTokenLoad = null;
        }

        #region CommandMetod

        private void AddProduct()
        {
            DialogService.ShowDialog("Catalog", new DialogParameters(), Callback);
        }

        private void Callback(IDialogResult dialogResult)
        {
            Product temp = dialogResult.Parameters.GetValue<Product>("product");
            if (temp != null) InsertProduct(temp);
        }

        private void DeleteProduct(Collection<object> obj)
        {
            List<TMovementGoodInfo> list = obj.Cast<TMovementGoodInfo>().ToList();
            list.ForEach(item => MovementGoodsInfosList.Remove(item));
        }

        private async void InsertProduct(Product product)
        {
            try
            {
                if (MovementGoodsInfosList.Any(objRev => objRev.Product.Id == product.Id)) return;

                TMovementGoodInfo temp = await CreateMovementGoodInfoAsync(product);

                MovementGoodsInfosList.Add(temp);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected abstract Task<TMovementGoodInfo> CreateMovementGoodInfoAsync(Product product);

        private async void Post()
        {
            if (MessageBox.Show(_askPost, "Проведение отчета", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;

            SqlMovementGoodsRepository movementGoodsRepository = new SqlMovementGoodsRepository();
            try
            {
                using (var transaction = movementGoodsRepository.Db.Database.BeginTransaction())
                {
                    try
                    {
                        MovementGoods temp = await PreparingReport();

                        temp.DateClose = null;
                        temp.DateCreate = null;

                        var t = temp.MovementGoodsInfosCollection;
                        temp.MovementGoodsInfosCollection = null;

                        await movementGoodsRepository.CreateAsync(temp);
                        //temp.SerialNumberLogsCollection = null;
                        temp.MovementGoodsInfosCollection = t;
                        await movementGoodsRepository.UpdateAsync(temp);
                        transaction.Commit();

                        MessageBox.Show("Отчет добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NewReport();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        if (ex.InnerException is SqlException sex && (sex.State == 2 || sex.State == 3))
                        {
                            if (MovementGoodsReport.IsValid)
                            {
                                Post();
                            }
                            else
                            {
                                RaisePropertyChanged("MovementGoodsReport");
                                MessageBox.Show("Произошла ошибка проверьте данные", "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                            return;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected abstract Task<MovementGoods> PreparingReport();

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ReloadAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            CancelTokenLoad?.Cancel();
            CancelTokenNewReport?.Cancel();
        }

        #endregion
    }
}

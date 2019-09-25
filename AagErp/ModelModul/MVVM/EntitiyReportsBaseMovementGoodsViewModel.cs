using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class EntitiyReportsBaseMovementGoodsViewModel : EntitiyReportsViewModel<MovementGoods>
    {
        #region Properties

        private readonly string _movementGoodType;

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

        #endregion

        protected EntitiyReportsBaseMovementGoodsViewModel(IDialogService dialogService, string movementGoodType) : base(dialogService)
        {
            _movementGoodType = movementGoodType;
        }

        protected abstract Task PreLoadAsync();

        protected override async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            await PreLoadAsync();

            try
            {
                IRepository<MovementGoods> repository = new SqlMovementGoodsRepository();
                ReportsList = new ObservableCollection<MovementGoods>(await repository.GetListAsync(
                    CancelTokenSource.Token,
                    MovementGoodsReportSpecification.GetMovementGoodsReportsByType(_movementGoodType),
                    new Dictionary<string, SortingTypes> {{"DateCreate", SortingTypes.DESC}}, Left, Step,
                    (m => m.MovementGoodsInfosCollection, new Expression<Func<object, object>>[]
                        {m => (m as MovementGoodsInfo).Product, p => (p as Product).UnitStorage})));
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
        }
    }
}

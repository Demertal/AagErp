using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace CounterpartyModul.ViewModels
{
    public class ShowCounterpartiesViewModel : EntitiesViewModelBase<Counterparty, SqlCounterpartyRepository>
    {
        private readonly ObservableCollection<CounterpartyType> _counterpartyTypesList =
            new ObservableCollection<CounterpartyType>
            {
                new CounterpartyType(ETypeCounterparties.Buyers, "Покупатель"),
                new CounterpartyType(ETypeCounterparties.Suppliers, "Поставщик")
            };
        public ObservableCollection<CounterpartyType> CounterpartyTypesList => _counterpartyTypesList;

        private ObservableCollection<PaymentType> _paymentTypesList = new ObservableCollection<PaymentType>();
        public ObservableCollection<PaymentType> PaymentTypesList
        {
            get => _paymentTypesList;
            set => SetProperty(ref _paymentTypesList, value);
        }

        public ShowCounterpartiesViewModel(IDialogService dialogService) : base(dialogService, "ShowCounterparty", "Удалить контрагента?", "Контрагент удален.")
        {
        }

        protected override async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                IRepository<PaymentType> paymentTypeRepository = new SqlPaymentTypeRepository();

                var counterpartyLoad = Task.Run(() => counterpartyRepository.GetListAsync(CancelTokenSource.Token), CancelTokenSource.Token);
                var paymentTypeLoad = Task.Run(() => paymentTypeRepository.GetListAsync(CancelTokenSource.Token), CancelTokenSource.Token);

                await Task.WhenAll(counterpartyLoad, paymentTypeLoad);

                EntitiesList = new ObservableCollection<Counterparty>(counterpartyLoad.Result);
                PaymentTypesList = new ObservableCollection<PaymentType>(paymentTypeLoad.Result);

                if (EntitiesList != null)
                    foreach (var entity in EntitiesList)
                    {
                        entity.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                    }
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
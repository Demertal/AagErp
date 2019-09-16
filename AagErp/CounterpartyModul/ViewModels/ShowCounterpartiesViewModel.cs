using System.Collections.ObjectModel;
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
            LoadAsync();
        }

        public async void LoadAsync()
        {
            IRepository<PaymentType> paymentTypeRepository = new SqlPaymentTypeRepository();
            PaymentTypesList = new ObservableCollection<PaymentType>(await paymentTypeRepository.GetListAsync());
        }
    }
}
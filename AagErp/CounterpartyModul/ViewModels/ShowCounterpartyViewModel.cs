using System.Collections.ObjectModel;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;

namespace CounterpartyModul.ViewModels
{
    public class ShowCounterpartyViewModel : EntityViewModelBase<Counterparty, CounterpartyViewModel, SqlCounterpartyRepository>
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

        public ShowCounterpartyViewModel() : base("Контрагент добавлен", "Контрагент изменен")
        {
            LoadAsync();
        }

        public async void LoadAsync()
        {
            IRepository<PaymentType> paymentTypeRepository = new SqlPaymentTypeRepository();
            PaymentTypesList = new ObservableCollection<PaymentType>(await paymentTypeRepository.GetListAsync());
        }

        public override void PropertiesTransfer(Counterparty fromEntity, Counterparty toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
            toEntity.Address = fromEntity.Address;
            toEntity.ContactPerson = fromEntity.ContactPerson;
            toEntity.ContactPhone = fromEntity.ContactPhone;
            toEntity.IdPaymentType = fromEntity.IdPaymentType;
            toEntity.Props = fromEntity.Props;
            toEntity.WhoIsIt = fromEntity.WhoIsIt;
        }
    }
}
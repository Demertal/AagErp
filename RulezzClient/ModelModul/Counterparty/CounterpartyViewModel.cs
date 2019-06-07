using Prism.Mvvm;

namespace ModelModul.Counterparty
{
    public class CounterpartyViewModel :BindableBase
    {
        private Counterparties _counterparty = new Counterparties();

        public Counterparties Counterparty
        {
            get => _counterparty;
            set
            {
                SetProperty(ref _counterparty, value);
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Address");
                RaisePropertyChanged("ContactPerson");
                RaisePropertyChanged("ContactPhone");
                RaisePropertyChanged("Props");
                RaisePropertyChanged("IsValidate");
            }
        }

        public int Id
        {
            get => _counterparty.Id;
            set
            {
                _counterparty.Id = value;
                RaisePropertyChanged();
            }
        }

        public bool WhoIsIt
        {
            get => _counterparty.WhoIsIt;
            set
            {
                _counterparty.WhoIsIt = value;
                RaisePropertyChanged();
            }
        }

        public TypeCounterparties TypeCounterparty
        {
            get => _counterparty.WhoIsIt ? TypeCounterparties.Buyers : TypeCounterparties.Suppliers;
            set
            {
                _counterparty.WhoIsIt = value.HasFlag(TypeCounterparties.Buyers);
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get => _counterparty.Title;
            set
            {
                _counterparty.Title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public string Address
        {
            get => _counterparty.Address;
            set
            {
                _counterparty.Address = value;
                RaisePropertyChanged();
            }
        }

        public string ContactPerson
        {
            get => _counterparty.ContactPerson;
            set
            {
                _counterparty.ContactPerson = value;
                RaisePropertyChanged();
            }
        }

        public string ContactPhone
        {
            get => _counterparty.ContactPhone;
            set
            {
                _counterparty.ContactPhone = value;
                RaisePropertyChanged();
            }
        }

        public string Props
        {
            get => _counterparty.Props;
            set
            {
                _counterparty.Props = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title) && Title != "Покупатель";
    }
}

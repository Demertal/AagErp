//using ModelModul.Models;
//using Prism.Mvvm;

//namespace ModelModul
//{
//    public class CounterpartyViewModel :BindableBase
//    {
//        private Counterparty _counterparty = new Counterparty();

//        public Counterparty Counterparty
//        {
//            get => _counterparty;
//            set
//            {
//                SetProperty(ref _counterparty, value);
//                RaisePropertyChanged("Title");
//                RaisePropertyChanged("Address");
//                RaisePropertyChanged("ContactPerson");
//                RaisePropertyChanged("ContactPhone");
//                RaisePropertyChanged("Props");
//                RaisePropertyChanged("Debt");
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public int Id
//        {
//            get => _counterparty.Id;
//            set
//            {
//                _counterparty.Id = value;
//                RaisePropertyChanged();
//            }
//        }

//        public TypeCounterparties WhoIsIt
//        {
//            get => _counterparty.WhoIsIt;
//            set
//            {
//                _counterparty.WhoIsIt = value;
//                RaisePropertyChanged();
//            }
//        }

//        public TypeCounterparties TypeCounterparty
//        {
//            get => _counterparty.WhoIsIt;
//            set
//            {
//                _counterparty.WhoIsIt = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string Title
//        {
//            get => _counterparty.Title;
//            set
//            {
//                _counterparty.Title = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public string Address
//        {
//            get => _counterparty.Address;
//            set
//            {
//                _counterparty.Address = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string ContactPerson
//        {
//            get => _counterparty.ContactPerson;
//            set
//            {
//                _counterparty.ContactPerson = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string ContactPhone
//        {
//            get => _counterparty.ContactPhone;
//            set
//            {
//                _counterparty.ContactPhone = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string Props
//        {
//            get => _counterparty.Props;
//            set
//            {
//                _counterparty.Props = value;
//                RaisePropertyChanged();
//            }
//        }

//        public decimal Debt
//        {
//            get => _counterparty.Debt;
//            set
//            {
//                _counterparty.Debt = value;
//                RaisePropertyChanged();
//            }
//        }

//        public bool IsValidate => !string.IsNullOrEmpty(Title) && Title != "Покупатель";
//    }
//}

using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Counterparty : ModelBase
    {
        public Counterparty()
        {
            MoneyTransfersCollection = new List<MoneyTransfer>();
            MovementGoodsCollection = new List<MovementGoods>();
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("IsValidate");
            }
        }

        private string _contactPerson;
        public string ContactPerson
        {
            get => _contactPerson;
            set
            {
                _contactPerson = value;
                OnPropertyChanged("ContactPerson");
            }
        }

        private string _contactPhone;
        public string ContactPhone
        {
            get => _contactPhone;
            set
            {
                _contactPhone = value;
                OnPropertyChanged("ContactPhone");
            }
        }

        private string _props;
        public string Props
        {
            get => _props;
            set
            {
                _props = value;
                OnPropertyChanged("Props");
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private ETypeCounterparties _whoIsIt;
        public virtual ETypeCounterparties WhoIsIt
        {
            get => _whoIsIt;
            set
            {
                _whoIsIt = value;
                OnPropertyChanged("WhoIsIt");
            }
        }

        private int _idPaymentType;
        public int IdPaymentType
        {
            get => _idPaymentType;
            set
            {
                _idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
                OnPropertyChanged("IsValidate");
            }
        }

        private PaymentType _paymentType;
        public virtual PaymentType PaymentType
        {
            get => _paymentType;
            set
            {
                _paymentType = value;
                OnPropertyChanged("PaymentType");
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfersCollection;
        public virtual ICollection<MoneyTransfer> MoneyTransfersCollection
        {
            get => _moneyTransfersCollection;
            set
            {
                _moneyTransfersCollection = value;
                OnPropertyChanged("MoneyTransfersCollection");
            }
        }

        private ICollection<MovementGoods> _movementGoodsCollection;
        public virtual ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged("MovementGoodsCollection");
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                        {
                            error = "Наименование должно быть указано";
                        }

                        break;

                    case "IdPaymentType":
                        if (IdPaymentType == 0)
                        {
                            error = "Тип оплаты должен быть указан";
                        }

                        break;
                }

                return error;
            }
        }

        public override bool IsValidate => !string.IsNullOrEmpty(Title) && IdPaymentType != 0;

        public override object Clone()
        {
            return new Counterparty
            {
                Id = Id,
                Address = Address,
                ContactPerson = ContactPerson,
                ContactPhone = ContactPhone,
                IdPaymentType = IdPaymentType,
                Props = Props,
                Title = Title,
                WhoIsIt = WhoIsIt
            };
        }
    }
}

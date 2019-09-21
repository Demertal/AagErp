using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Counterparty : ModelBase<Counterparty>
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
                OnPropertyChanged();
            }
        }

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _contactPerson;
        public string ContactPerson
        {
            get => _contactPerson;
            set
            {
                _contactPerson = value;
                OnPropertyChanged();
            }
        }

        private string _contactPhone;
        public string ContactPhone
        {
            get => _contactPhone;
            set
            {
                _contactPhone = value;
                OnPropertyChanged();
            }
        }

        private string _props;
        public string Props
        {
            get => _props;
            set
            {
                _props = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private ETypeCounterparties _whoIsIt;
        public virtual ETypeCounterparties WhoIsIt
        {
            get => _whoIsIt;
            set
            {
                _whoIsIt = value;
                OnPropertyChanged();
            }
        }

        private int _idPaymentType;
        public int IdPaymentType
        {
            get => _idPaymentType;
            set
            {
                _idPaymentType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private PaymentType _paymentType;
        public virtual PaymentType PaymentType
        {
            get => _paymentType;
            set
            {
                _paymentType = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfersCollection;
        public virtual ICollection<MoneyTransfer> MoneyTransfersCollection
        {
            get => _moneyTransfersCollection;
            set
            {
                _moneyTransfersCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MovementGoods> _movementGoodsCollection;
        public virtual ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged();
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

        public override bool IsValid => !string.IsNullOrEmpty(Title) && IdPaymentType > 0 && !HasErrors;
    }
}
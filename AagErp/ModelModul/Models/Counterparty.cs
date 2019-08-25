using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Counterparty : ModelBase, ICloneable
    {
        public Counterparty()
        {
            MoneyTransfers = new List<MoneyTransfer>();
            MovementGoods = new List<MovementGoods>();
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
        public string Title
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

        private TypeCounterparties _whoIsIt;
        public TypeCounterparties WhoIsIt
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

        public bool IsValidate => !string.IsNullOrEmpty(Title) && IdPaymentType != 0;

        private ICollection<MoneyTransfer> _moneyTransfers;
        public virtual ICollection<MoneyTransfer> MoneyTransfers
        {
            get => _moneyTransfers;
            set
            {
                _moneyTransfers = value;
                OnPropertyChanged("MoneyTransfers");
            }
        }

        private ICollection<MovementGoods> _movementGoods;
        public virtual ICollection<MovementGoods> MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged("MovementGoods");
            }
        }

        public object Clone()
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

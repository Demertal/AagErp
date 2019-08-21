using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelModul.Models
{
    public class MoneyTransfer : ModelBase
    {
        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private int _idCounterparty;
        public int IdCounterparty
        {
            get => _idCounterparty;
            set
            {
                _idCounterparty = value;
                OnPropertyChanged("IdCounterparty");
            }
        }

        private decimal _moneyAmount;
        [Column(TypeName = "money")]
        public decimal MoneyAmount
        {
            get => _moneyAmount;
            set
            {
                _moneyAmount = value;
                OnPropertyChanged("MoneyAmount");
            }
        }

        private int _idType;
        public int IdType
        {
            get => _idType;
            set
            {
                _idType = value;
                OnPropertyChanged("IdType");
            }
        }

        private Guid _idMovementGoods;
        public Guid IdMovementGoods
        {
            get => _idMovementGoods;
            set
            {
                _idMovementGoods = value;
                OnPropertyChanged("IdMovementGoods");
            }
        }

        private Counterparty _ñounterparty;
        public virtual Counterparty Counterparty
        {
            get => _ñounterparty;
            set
            {
                _ñounterparty = value;
                OnPropertyChanged("Counterparty");
            }
        }

        private MovementGoods _movementGoods;
        public virtual MovementGoods MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged("MovementGoods");
            }
        }

        private MoneyTransferType _moneyTransferType;
        public MoneyTransferType MoneyTransferType
        {
            get => _moneyTransferType;
            set
            {
                _moneyTransferType = value;
                OnPropertyChanged("MoneyTransferType");
            }
        }
    }
}

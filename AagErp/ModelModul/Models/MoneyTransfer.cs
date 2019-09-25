using System;

namespace ModelModul.Models
{
    public class MoneyTransfer : ModelBase<MoneyTransfer>
    {
        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private int _idCounterparty;
        public int IdCounterparty
        {
            get => _idCounterparty;
            set
            {
                _idCounterparty = value;
                OnPropertyChanged();
            }
        }

        private decimal _moneyAmount;
        public decimal MoneyAmount
        {
            get => _moneyAmount;
            set
            {
                _moneyAmount = value;
                OnPropertyChanged();
            }
        }

        private int _idType;
        public int IdType
        {
            get => _idType;
            set
            {
                _idType = value;
                OnPropertyChanged();
            }
        }

        private long _idMovementGoods;
        public long IdMovementGoods
        {
            get => _idMovementGoods;
            set
            {
                _idMovementGoods = value;
                OnPropertyChanged();
            }
        }

        private Counterparty _ñounterparty;
        public virtual Counterparty Counterparty
        {
            get => _ñounterparty;
            set
            {
                _ñounterparty = value;
                OnPropertyChanged();
            }
        }

        private MovementGoods _movementGoods;
        public virtual MovementGoods MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged();
            }
        }

        private MoneyTransferType _moneyTransferType;
        public MoneyTransferType MoneyTransferType
        {
            get => _moneyTransferType;
            set
            {
                _moneyTransferType = value;
                OnPropertyChanged();
            }
        }

        public override bool IsValid => true;
    }
}

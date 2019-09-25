using System;

namespace ModelModul.Models
{
    public class SerialNumberLog : ModelBase<SerialNumberLog>
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

        private long _idSerialNumber;
        public long IdSerialNumber
        {
            get => _idSerialNumber;
            set
            {
                _idSerialNumber = value;
                OnPropertyChanged();
            }
        }

        private long _idMovmentGood;
        public long IdMovmentGood
        {
            get => _idMovmentGood;
            set
            {
                _idMovmentGood = value;
                OnPropertyChanged();
            }
        }

        private MovementGoods _movementGood;
        public virtual MovementGoods MovementGood
        {
            get => _movementGood;
            set
            {
                _movementGood = value;
                OnPropertyChanged();
            }
        }

        private SerialNumber _serialNumber;
        public virtual SerialNumber SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        public override bool IsValid => true;
    }
}

using System;

namespace ModelModul.Models
{
    public class SerialNumberLog : ModelBase
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

        private long _idSerialNumber;
        public long IdSerialNumber
        {
            get => _idSerialNumber;
            set
            {
                _idSerialNumber = value;
                OnPropertyChanged("IdSerialNumber");
            }
        }

        private Guid _idMovmentGood;
        public Guid IdMovmentGood
        {
            get => _idMovmentGood;
            set
            {
                _idMovmentGood = value;
                OnPropertyChanged("IdMovmentGood");
            }
        }

        private MovementGoods _movementGood;
        public virtual MovementGoods MovementGood
        {
            get => _movementGood;
            set
            {
                _movementGood = value;
                OnPropertyChanged("MovementGood");
            }
        }

        private SerialNumber _serialNumber;
        public virtual SerialNumber SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }
    }
}

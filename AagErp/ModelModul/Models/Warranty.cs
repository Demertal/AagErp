using System;

namespace ModelModul.Models
{
    public class Warranty : ModelBase<Warranty>
    {
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

        private string _malfunction;
        public string Malfunction
        {
            get => _malfunction;
            set
            {
                _malfunction = value;
                OnPropertyChanged("Malfunction");
            }
        }

        private DateTime _dateReceipt;
        public DateTime DateReceipt
        {
            get => _dateReceipt;
            set
            {
                _dateReceipt = value;
                OnPropertyChanged("DateReceipt");
            }
        }

        private DateTime? _dateDeparture;
        public DateTime? DateDeparture
        {
            get => _dateDeparture;
            set
            {
                _dateDeparture = value;
                OnPropertyChanged("DateDeparture");
            }
        }

        private DateTime? _dateIssue;
        public DateTime? DateIssue
        {
            get => _dateIssue;
            set
            {
                _dateIssue = value;
                OnPropertyChanged("DateIssue");
            }
        }

        private string _info;
        public string Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged("Info");
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

        private long _idSerialNumberÑhange;
        public long IdSerialNumberÑhange
        {
            get => _idSerialNumberÑhange;
            set
            {
                _idSerialNumberÑhange = value;
                OnPropertyChanged("IdSerialNumberÑhange");
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

        private SerialNumber _serialNumberÑhange;
        public virtual SerialNumber SerialNumberÑhange
        {
            get => _serialNumberÑhange;
            set
            {
                _serialNumberÑhange = value;
                OnPropertyChanged("SerialNumberÑhange");
            }
        }

        public override bool IsValid => true;
    }
}

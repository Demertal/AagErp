using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelModul.Models
{
    public class Warranty : ModelBase
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
        [Required]
        [StringLength(256)]
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
        [Column(TypeName = "date")]
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
        [Column(TypeName = "date")]
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
        [Column(TypeName = "date")]
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
        [StringLength(256)]
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

        private long _idSerialNumber—hange;
        public long IdSerialNumber—hange
        {
            get => _idSerialNumber—hange;
            set
            {
                _idSerialNumber—hange = value;
                OnPropertyChanged("IdSerialNumber—hange");
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

        private SerialNumber _serialNumber—hange;
        public virtual SerialNumber SerialNumber—hange
        {
            get => _serialNumber—hange;
            set
            {
                _serialNumber—hange = value;
                OnPropertyChanged("SerialNumber—hange");
            }
        }
    }
}

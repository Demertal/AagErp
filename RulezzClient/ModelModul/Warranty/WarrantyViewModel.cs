using System;
using Prism.Mvvm;

namespace ModelModul.Warranty
{
    public class WarrantyViewModel: BindableBase
    {
        private Warranties _warranty = new Warranties();

        public Warranties Warranty
        {
            get => _warranty;
            set
            {
                SetProperty(ref _warranty, value);
                RaisePropertyChanged("Malfunction");
                RaisePropertyChanged("DateDeparture");
                RaisePropertyChanged("DateIssue");
                RaisePropertyChanged("DateReceipt");
                RaisePropertyChanged("Info");
                RaisePropertyChanged("IsValidate");
            }
        }

        public string Malfunction
        {
            get => _warranty.Malfunction;
            set
            {
                _warranty.Malfunction = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public string Info
        {
            get => _warranty.Info;
            set
            {
                _warranty.Info = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? DateDeparture
        {
            get => _warranty.DateDeparture;
            set
            {
                _warranty.DateDeparture = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? DateIssue
        {
            get => _warranty.DateIssue;
            set
            {
                _warranty.DateIssue = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? DateReceipt
        {
            get => _warranty.DateReceipt;
            set
            {
                _warranty.DateReceipt = value;
                RaisePropertyChanged();
            }
        }

        public int IdSerialNumber
        {
            get => _warranty.IdSerialNumber;
            set
            {
                _warranty.IdSerialNumber = value;
                RaisePropertyChanged();
            }
        }

        public SerialNumbers SerialNumber
        {
            get => _warranty.SerialNumbers;
            set
            {
                _warranty.SerialNumbers = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Malfunction);
    }
}

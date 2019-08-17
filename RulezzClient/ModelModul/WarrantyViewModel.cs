//using System;
//using ModelModul.Models;
//using Prism.Mvvm;

//namespace ModelModul
//{
//    public class WarrantyViewModel: BindableBase
//    {
//        private Warranty _warranty = new Warranty();

//        public Warranty Warranty
//        {
//            get => _warranty;
//            set
//            {
//                SetProperty(ref _warranty, value);
//                RaisePropertyChanged("Malfunction");
//                RaisePropertyChanged("DateDeparture");
//                RaisePropertyChanged("DateIssue");
//                RaisePropertyChanged("DateReceipt");
//                RaisePropertyChanged("Info");
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public string Malfunction
//        {
//            get => _warranty.Malfunction;
//            set
//            {
//                _warranty.Malfunction = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public string Info
//        {
//            get => _warranty.Info;
//            set
//            {
//                _warranty.Info = value;
//                RaisePropertyChanged();
//            }
//        }

//        public DateTime? DateDeparture
//        {
//            get => _warranty.DateDeparture;
//            set
//            {
//                _warranty.DateDeparture = value;
//                RaisePropertyChanged();
//            }
//        }

//        public DateTime? DateIssue
//        {
//            get => _warranty.DateIssue;
//            set
//            {
//                _warranty.DateIssue = value;
//                RaisePropertyChanged();
//            }
//        }

//        public DateTime? DateReceipt
//        {
//            get => _warranty.DateReceipt;
//            set
//            {
//                _warranty.DateReceipt = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int IdSerialNumber
//        {
//            get => _warranty.IdSerialNumber;
//            set
//            {
//                _warranty.IdSerialNumber = value;
//                RaisePropertyChanged();
//            }
//        }

//        public SerialNumber SerialNumber
//        {
//            get => _warranty.SerialNumber;
//            set
//            {
//                _warranty.SerialNumber = value;
//                RaisePropertyChanged();
//            }
//        }

//        public bool IsValidate => !string.IsNullOrEmpty(Malfunction);
//    }
//}

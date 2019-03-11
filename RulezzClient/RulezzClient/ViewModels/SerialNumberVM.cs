//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class SerialNumberVM : BindableBase
//    {
//        private SerialNumbers _serialNumber;

//        public SerialNumberVM()
//        {

//        }

//        public SerialNumberVM(SerialNumbers serialNumber)
//        {
//            SerialNumber = serialNumber;
//        }

//        public SerialNumbers SerialNumber
//        {
//            get => _serialNumber;
//            set
//            {
//                _serialNumber = value;
//                RaisePropertyChanged();
//            }
//        }
//        public int Id
//        {
//            get => SerialNumber.Id;
//            set
//            {
//                SerialNumber.Id = value;
//                RaisePropertyChanged();
//            }

//        }
//        public string Value
//        {
//            get => SerialNumber.Value;
//            set
//            {
//                SerialNumber.Value = value;
//                RaisePropertyChanged();
//            }

//        }
//        public DateTime? SelleDate
//        {
//            get => SerialNumber.SelleDate;
//            set
//            {
//                SerialNumber.SelleDate = value;
//                RaisePropertyChanged();
//            }

//        }
//        public DateTime PurchaseDate
//        {
//            get => SerialNumber.PurchaseDate;
//            set
//            {
//                SerialNumber.PurchaseDate = value;
//                RaisePropertyChanged();
//            }

//        }
//        public int IdProduct
//        {
//            get => SerialNumber.IdProduct;
//            set
//            {
//                SerialNumber.IdProduct = value;
//                RaisePropertyChanged();
//            }

//        }
//    }
//}

//using ModelModul.Models;
//using Prism.Mvvm;

//namespace ModelModul
//{
//    public class SerialNumberViewModel : BindableBase
//    {
//        private SerialNumber _serialNumber;

//        public SerialNumber SerialNumber
//        {
//            get => _serialNumber;
//            set
//            {
//                SetProperty(ref _serialNumber, value);
//                RaisePropertyChanged("Id");
//                RaisePropertyChanged("IdProduct");
//                RaisePropertyChanged("IdPurchaseReport");
//                RaisePropertyChanged("IdSaleReport");
//                RaisePropertyChanged("Value");
//                RaisePropertyChanged("Purchase");
//                RaisePropertyChanged("Sales");
//                RaisePropertyChanged("Product");
//            }
//        }

//        public int Id
//        {
//            get => _serialNumber.Id;
//            set
//            {
//                _serialNumber.Id = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int IdProduct
//        {
//            get => _serialNumber.IdProduct;
//            set
//            {
//                _serialNumber.IdProduct = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int IdPurchaseReport
//        {
//            get => _serialNumber.IdPurchaseReport;
//            set
//            {
//                _serialNumber.IdPurchaseReport = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int? IdSaleReport
//        {
//            get => _serialNumber.IdSaleReport;
//            set
//            {
//                _serialNumber.IdSaleReport = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string Value
//        {
//            get => _serialNumber.Value;
//            set
//            {
//                _serialNumber.Value = value;
//                RaisePropertyChanged();
//            }
//        }

//        public MovementGoods Purchase
//        {
//            get => _serialNumber.Purchase;
//            set
//            {
//                _serialNumber.Purchase = value;
//                RaisePropertyChanged();
//            }
//        }

//        public MovementGoods Sales
//        {
//            get => _serialNumber.Sales;
//            set
//            {
//                _serialNumber.Sales = value;
//                RaisePropertyChanged();
//            }
//        }

//        public Product Product
//        {
//            get => _serialNumber.Product;
//            set
//            {
//                _serialNumber.Product = value;
//                RaisePropertyChanged();
//            }
//        }
//    }
//}

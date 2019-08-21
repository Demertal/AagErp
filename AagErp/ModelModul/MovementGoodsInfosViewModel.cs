//using System.Collections.ObjectModel;
//using System.Linq;
//using ModelModul.Models;
//using Prism.Commands;

//namespace ModelModul
//{
//    public class MovementGoodsInfosViewModel : ProductViewModel
//    {
//        private MovementGoodsInfo _movementGoodsInfo;

//        public MovementGoodsInfo MovementGoodsInfo
//        {
//            get => _movementGoodsInfo;
//            set => SetProperty(ref _movementGoodsInfo, value);
//        }

//        public override int Id
//        {
//            get => _movementGoodsInfo.Id;
//            set
//            {
//                _movementGoodsInfo.Id = value;
//                RaisePropertyChanged();
//            }
//        }

//        public new double Count
//        {
//            get => _movementGoodsInfo.Count;
//            set
//            {
//                _movementGoodsInfo.Count = value;
//                if (Product.KeepTrackSerialNumbers)
//                {
//                    while (Count > SerialNumbers.Count)
//                    {
//                        SerialNumbers.Add(new SerialNumber {IdProduct = Product.Id});
//                    }

//                    while (Count < SerialNumbers.Count)
//                    {
//                        SerialNumber sr = SerialNumbers.FirstOrDefault(ser => string.IsNullOrEmpty(ser.Value)) ?? SerialNumbers.Last();
//                        SerialNumbers.Remove(sr);
//                    }

//                    if (SerialNumbers.FirstOrDefault(objSer => string.IsNullOrEmpty(objSer.Value)) != null)
//                        IsExpanded = true;
//                }
//                RaisePropertyChanged("IsValidate");
//                RaisePropertyChanged();
//            }
//        }

//        public decimal? Price
//        {
//            get => _movementGoodsInfo.Price;
//            set
//            {
//                _movementGoodsInfo.Price = value;
//                RaisePropertyChanged("IsValidate");
//                RaisePropertyChanged();
//            }
//        }

//        public int IdReport
//        {
//            get => _movementGoodsInfo.IdReport;
//            set
//            {
//                _movementGoodsInfo.IdReport = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int IdProduct
//        {
//            get => _movementGoodsInfo.IdProduct;
//            set
//            {
//                _movementGoodsInfo.IdProduct = value;
//                RaisePropertyChanged();
//            }
//        }

//        public override Product Product
//        {
//            get => _movementGoodsInfo.Product;
//            set
//            {
//                _movementGoodsInfo.Product = value;
//                SerialNumbers = new ObservableCollection<SerialNumber>();
//                RaisePropertyChanged();
//            }
//        }

//        public virtual MovementGoods MovementGoods
//        {
//            get => _movementGoodsInfo.MovementGoods;
//            set
//            {
//                _movementGoodsInfo.MovementGoods = value;
//                RaisePropertyChanged();
//            }
//        }

//        //public override ObservableCollection<SerialNumber> SerialNumber
//        //{
//        //    get => PurchaseInfo.SerialNumber as ObservableCollection<SerialNumber>;
//        //    set
//        //    {
//        //        PurchaseInfo.SerialNumber = value;
//        //        SerialNumber.CollectionChanged += SerialNumbersCollectionChanged;
//        //        RaisePropertyChanged();
//        //    }
//        //}

//        public override bool IsValidate
//        {
//            get
//            {
//                //if (Count == 0 || PurchasePrice <= 0 || SerialNumber == null) return false;
//                foreach (var serialNumber in SerialNumbers)
//                {
//                    if (string.IsNullOrEmpty(serialNumber.Value)) return false;
//                }
//                return true;
//            }
//        }

//        private bool _isExpanded;
//        public bool IsExpanded
//        {
//            get => _isExpanded;
//            set => SetProperty(ref _isExpanded, value);
//        }

//        public DelegateCommand<int?> ReloadSerialNumberCommand { get; }

//        public MovementGoodsInfosViewModel()
//        {
//            //PurchaseInfo = new PurchaseInfos{ Product = new Product() };
//            ReloadSerialNumberCommand = new DelegateCommand<int?>(ReloadSerialNumber);
//            SerialNumbers = new ObservableCollection<SerialNumber>();
//        }

//        private void ReloadSerialNumber(int? obj)
//        {
//            if (obj == null) return;
//            SerialNumbers[obj.Value].Value = "";
//            RaisePropertyChanged("SerialNumber");
//        }
//    }
//}

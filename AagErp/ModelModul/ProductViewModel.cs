//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using ModelModul.Models;
//using Prism.Mvvm;

//namespace ModelModul
//{
//    public class ProductViewModel: BindableBase
//    {
//        #region Properties

//        private Product _product = new Product();

//        public virtual Product Product
//        {
//            get => _product;
//            set
//            {
//                _product = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("Id");
//                RaisePropertyChanged("Title");
//                RaisePropertyChanged("VendorCode");
//                RaisePropertyChanged("Barcode");
//                RaisePropertyChanged("IdPriceGroup");
//                RaisePropertyChanged("IdWarrantyPeriod");
//                RaisePropertyChanged("IdCategory");
//                RaisePropertyChanged("IdUnitStorage");
//                RaisePropertyChanged("Parent");
//                RaisePropertyChanged("PriceGroup");
//                RaisePropertyChanged("CountProduct");
//                RaisePropertyChanged("UnitStorage");
//                RaisePropertyChanged("WarrantyPeriod");
//                RaisePropertyChanged("PropertyProduct");
//                RaisePropertyChanged("MovementGoodsInfos");
//                RaisePropertyChanged("PriceProduct");
//                RaisePropertyChanged("SerialNumber");
//                RaisePropertyChanged("KeepTrackSerialNumbers");
//                RaisePropertyChanged("Count");
//            }
//        }

//        public virtual int Id
//        {
//            get => Product.Id;
//            set
//            {
//                Product.Id = value;
//                RaisePropertyChanged();
//            }
//        }

//        public string Title
//        {
//            get => Product.Title;
//            set
//            {
//                Product.Title = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }
//        public string VendorCode
//        {
//            get => Product.VendorCode;
//            set
//            {
//                Product.VendorCode = value;
//                RaisePropertyChanged();
//            }
//        }
//        public string Barcode
//        {
//            get => Product.Barcode;
//            set
//            {
//                Product.Barcode = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public int IdWarrantyPeriod
//        {
//            get => Product.IdWarrantyPeriod;
//            set
//            {
//                Product.IdWarrantyPeriod = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public int IdPriceGroup
//        {
//            get => Product.IdPriceGroup;
//            set
//            {
//                Product.IdPriceGroup = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }
//        public int IdGroup
//        {
//            get => Product.IdCategory;
//            set
//            {
//                Product.IdCategory = value;
//                RaisePropertyChanged();
//            }
//        }
//        public int IdUnitStorage
//        {
//            get => Product.IdUnitStorage;
//            set
//            {
//                Product.IdUnitStorage = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public bool KeepTrackSerialNumbers
//        {
//            get => Product.KeepTrackSerialNumbers;
//            set
//            {
//                Product.KeepTrackSerialNumbers = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsValidate");
//            }
//        }

//        public Category Category
//        {
//            get => Product.Category;
//            set
//            {
//                Product.Category = value;
//                RaisePropertyChanged();
//            }
//        }

//        public PriceGroup PriceGroup
//        {
//            get => Product.PriceGroup;
//            set
//            {
//                Product.PriceGroup = value;
//                RaisePropertyChanged();
//            }
//        }
//        public ICollection<CountProduct> CountProducts
//        {
//            get => Product.CountProducts;
//            set
//            {
//                Product.CountProducts = value;
//                RaisePropertyChanged();
//            }
//        }
//        public UnitStorage UnitStorage
//        {
//            get => Product.UnitStorage;
//            set
//            {
//                Product.UnitStorage = value;
//                RaisePropertyChanged();
//            }
//        }
//        public WarrantyPeriod WarrantyPeriod
//        {
//            get => Product.WarrantyPeriod;
//            set
//            {
//                Product.WarrantyPeriod = value;
//                RaisePropertyChanged();
//            }
//        }
//        public ICollection<PropertyProduct> PropertyProducts
//        {
//            get => Product.PropertyProducts;
//            set
//            {
//                Product.PropertyProducts = value;
//                RaisePropertyChanged();
//            }
//        }
//        public ICollection<MovementGoodsInfo> MovementGoodsInfos
//        {
//            get => Product.MovementGoodsInfos;
//            set
//            {
//                Product.MovementGoodsInfos = value;
//                RaisePropertyChanged();
//            }
//        }

//        public ICollection<PriceProduct> PriceProduct
//        {
//            get => Product.PriceProducts;
//            set
//            {
//                Product.PriceProducts = value;
//                RaisePropertyChanged();
//            }
//        }

//        public virtual ObservableCollection<SerialNumber> SerialNumbers
//        {
//            get => Product.SerialNumbers as ObservableCollection<SerialNumber>;
//            set
//            {
//                Product.SerialNumbers = value;
//                SerialNumbers.CollectionChanged += SerialNumbersCollectionChanged;
//                RaisePropertyChanged();
//            }
//        }

//        public virtual double Count
//        {
//            get
//            {
//                if (CountProducts == null || CountProducts.Count == 0) return 0;
//                double count = 0;
//                foreach (var ct in CountProducts)
//                {
//                    count += ct.Count;
//                }
//                return count;
//            }
//        }

//        public virtual bool IsValidate => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Barcode) && IdUnitStorage != 0 && IdWarrantyPeriod != 0;
//        #endregion

//        protected void SerialNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Remove:
//                    foreach (SerialNumber item in e.OldItems)
//                    {
//                        //Removed items
//                        item.PropertyChanged -= SerialNumbersViewModelPropertyChanged;
//                    }
//                    break;
//                case NotifyCollectionChangedAction.Add:
//                    foreach (SerialNumber item in e.NewItems)
//                    {
//                        //Added items
//                        item.PropertyChanged += SerialNumbersViewModelPropertyChanged;
//                    }

//                    break;
//            }

//            RaisePropertyChanged("IsValidate");
//        }

//        private void SerialNumbersViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            RaisePropertyChanged("IsValidate");
//        }
//    }
//}

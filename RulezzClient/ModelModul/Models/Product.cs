using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class Product : ModelBase, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            InvoiceInfos = new HashSet<InvoiceInfo>();
            MovementGoodsInfos = new HashSet<MovementGoodsInfo>();
            PriceProducts = new HashSet<PriceProduct>();
            PropertyProducts = new HashSet<PropertyProduct>();
            SerialNumbers = new HashSet<SerialNumber>();
        }

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

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("IsValidate");
            }
        }

        private string _vendorCode;
        public string VendorCode
        {
            get => _vendorCode;
            set
            {
                _vendorCode = value;
                OnPropertyChanged("VendorCode");
            }
        }

        private string _barcode;
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged("Barcode");
                OnPropertyChanged("IsValidate");
            }
        }

        private int _idWarrantyPeriod;
        public int IdWarrantyPeriod
        {
            get => _idWarrantyPeriod;
            set
            {
                _idWarrantyPeriod = value;
                OnPropertyChanged("IdWarrantyPeriod");
                OnPropertyChanged("IsValidate");
            }
        }

        private int _idCategory;
        public int IdCategory
        {
            get => _idCategory;
            set
            {
                _idCategory = value;
                OnPropertyChanged("IdCategory");
                OnPropertyChanged("IsValidate");
            }
        }

        private int? _idPriceGroup;
        public int? IdPriceGroup
        {
            get => _idPriceGroup;
            set
            {
                _idPriceGroup = value;
                OnPropertyChanged("IdPriceGroup");
            }
        }

        private int _idUnitStorage;
        public int IdUnitStorage
        {
            get => _idUnitStorage;
            set
            {
                _idUnitStorage = value;
                OnPropertyChanged("IdUnitStorage");
                OnPropertyChanged("IsValidate");
            }
        }

        private bool _keepTrackSerialNumbers;
        public bool KeepTrackSerialNumbers
        {
            get => _keepTrackSerialNumbers;
            set
            {
                _keepTrackSerialNumbers = value;
                OnPropertyChanged("KeepTrackSerialNumbers");
            }
        }

        private double _count;
        public double Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        private ObservableCollection<CountsProduct> _countsProduct;
        public ObservableCollection<CountsProduct> CountsProduct
        {
            get => _countsProduct;
            set
            {
                if(CountsProduct != null)
                    CountsProduct.CollectionChanged -= CountsProductCollectionChanged;
                _countsProduct = value;
                if (CountsProduct != null)
                    CountsProduct.CollectionChanged += CountsProductCollectionChanged;
                OnPropertyChanged("CountsProduct");
            }
        }

        private Category _category;
        public virtual Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        private ICollection<InvoiceInfo> _invoiceInfos;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceInfo> InvoiceInfos
        {
            get => _invoiceInfos;
            set
            {
                _invoiceInfos = value;
                OnPropertyChanged("InvoiceInfos");
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfos;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfos
        {
            get => _movementGoodsInfos;
            set
            {
                _movementGoodsInfos = value;
                OnPropertyChanged("MovementGoodsInfos");
            }
        }

        private PriceGroup _priceGroup;
        public virtual PriceGroup PriceGroup
        {
            get => _priceGroup;
            set
            {
                _priceGroup = value;
                OnPropertyChanged("PriceGroup");
            }
        }

        private ICollection<PriceProduct> _priceProducts;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceProduct> PriceProducts
        {
            get => _priceProducts;
            set
            {
                _priceProducts = value;
                OnPropertyChanged("PriceProducts");
            }
        }

        private UnitStorage _unitStorage;
        public virtual UnitStorage UnitStorage
        {
            get => _unitStorage;
            set
            {
                _unitStorage = value;
                OnPropertyChanged("UnitStorage");
            }
        }

        private WarrantyPeriod _warrantyPeriod;
        public virtual WarrantyPeriod WarrantyPeriod
        {
            get => _warrantyPeriod;
            set
            {
                _warrantyPeriod = value;
                OnPropertyChanged("WarrantyPeriod");
            }
        }

        private ICollection<PropertyProduct> _propertyProducts;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyProduct> PropertyProducts
        {
            get => _propertyProducts;
            set
            {
                _propertyProducts = value;
                OnPropertyChanged("PropertyProducts");
            }
        }

        private ICollection<SerialNumber> _serialNumbers;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SerialNumber> SerialNumbers
        {
            get => _serialNumbers;
            set
            {
                _serialNumbers = value;
                OnPropertyChanged("SerialNumbers");
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title) &&
                                  !string.IsNullOrEmpty(Barcode) &&
                                  IdUnitStorage != 0 && IdWarrantyPeriod != 0 && IdCategory != 0;

        #region CollectionChanged

        private void CountsProductCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SerialNumber item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= CountsProductItemChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumber item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += CountsProductItemChanged;
                    }

                    break;
            }

            OnPropertyChanged("CountsProduct");
        }

        private void CountsProductItemChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CountsProduct");
        }

        #endregion

        public object Clone()
        {
            return new Product
            {
                Barcode = Barcode,
                VendorCode = VendorCode,
                Title = Title,
                Count = Count,
                Id = Id,
                IdCategory = IdCategory,
                IdPriceGroup = IdPriceGroup,
                IdUnitStorage = IdUnitStorage,
                IdWarrantyPeriod = IdWarrantyPeriod,
                KeepTrackSerialNumbers = KeepTrackSerialNumbers,
                Category = (Category) Category?.Clone(),
                UnitStorage = (UnitStorage) UnitStorage?.Clone(),
                WarrantyPeriod = (WarrantyPeriod) WarrantyPeriod?.Clone()
            };
        }
    }
}

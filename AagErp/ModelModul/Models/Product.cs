using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ModelModul.Models
{
    public class Product : ModelBase, ICloneable
    {
        public Product()
        {
            InvoiceInfos = new List<InvoiceInfo>();
            MovementGoodsInfos = new List<MovementGoodsInfo>();
            PriceProducts = new List<PriceProduct>();
            PropertyProducts = new List<PropertyProduct>();
            SerialNumbers = new List<SerialNumber>();
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

        private decimal _count;
        public decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        private decimal _price;
        public decimal Price
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

        private ObservableCollection<EquivalentCostFor≈xistingProduct> _equivalentCostFor≈xistingProducts;
        public virtual ObservableCollection<EquivalentCostFor≈xistingProduct> EquivalentCostFor≈xistingProducts
        {
            get => _equivalentCostFor≈xistingProducts;
            set
            {
                _equivalentCostFor≈xistingProducts = value;
                if(_equivalentCostFor≈xistingProducts != null)
                    _equivalentCostFor≈xistingProducts.CollectionChanged += EquivalentCostFor≈xistingProductsOnCollectionChanged;
                OnPropertyChanged("EquivalentCostFor≈xistingProducts");
            }
        }

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

        private void EquivalentCostFor≈xistingProductsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (EquivalentCostFor≈xistingProduct item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= EquivalentCostFor≈xistingProductItemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (EquivalentCostFor≈xistingProduct item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += EquivalentCostFor≈xistingProductItemPropertyChanged;
                    }

                    break;
            }

            OnPropertyChanged("EquivalentCostFor≈xistingProducts");
        }

        private void EquivalentCostFor≈xistingProductItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("EquivalentCostFor≈xistingProducts");
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
                WarrantyPeriod = (WarrantyPeriod) WarrantyPeriod?.Clone(),
                SerialNumbers = new List<SerialNumber>(SerialNumbers.Select(s => (SerialNumber)s.Clone()))
            };
        }
    }
}

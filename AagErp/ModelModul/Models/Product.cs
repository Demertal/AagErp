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
            InvoiceInfosCollection = new List<InvoiceInfo>();
            MovementGoodsInfosCollection = new List<MovementGoodsInfo>();
            PriceProductsCollection = new List<PriceProduct>();
            PropertyProductsCollection = new List<PropertyProduct>();
            SerialNumbersCollection = new List<SerialNumber>();
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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
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

        private int _idPriceGroup;
        public int IdPriceGroup
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

        private ObservableCollection<CountsProduct> _countsProductCollection;
        public ObservableCollection<CountsProduct> CountsProductCollection
        {
            get => _countsProductCollection;
            set
            {
                if(CountsProductCollection != null)
                    CountsProductCollection.CollectionChanged -= CountsProductCollectionChanged;
                _countsProductCollection = value;
                if (CountsProductCollection != null)
                    CountsProductCollection.CollectionChanged += CountsProductCollectionChanged;
                OnPropertyChanged("CountsProductCollection");
            }
        }

        private ICollection<InvoiceInfo> _invoiceInfosCollection;
        public virtual ICollection<InvoiceInfo> InvoiceInfosCollection
        {
            get => _invoiceInfosCollection;
            set
            {
                _invoiceInfosCollection = value;
                OnPropertyChanged("InvoiceInfosCollection");
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfosCollection;
        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfosCollection
        {
            get => _movementGoodsInfosCollection;
            set
            {
                _movementGoodsInfosCollection = value;
                OnPropertyChanged("MovementGoodsInfosCollection");
            }
        }

        private ICollection<PriceProduct> _priceProductsCollection;
        public virtual ICollection<PriceProduct> PriceProductsCollection
        {
            get => _priceProductsCollection;
            set
            {
                _priceProductsCollection = value;
                OnPropertyChanged("PriceProductsCollection");
            }
        }

        private ICollection<PropertyProduct> _propertyProductsCollection;
        public virtual ICollection<PropertyProduct> PropertyProductsCollection
        {
            get => _propertyProductsCollection;
            set
            {
                _propertyProductsCollection = value;
                OnPropertyChanged("PropertyProductsCollection");
            }
        }

        private ICollection<SerialNumber> _serialNumbersCollection;
        public virtual ICollection<SerialNumber> SerialNumbersCollection
        {
            get => _serialNumbersCollection;
            set
            {
                _serialNumbersCollection = value;
                OnPropertyChanged("SerialNumbersCollection");
            }
        }

        private ObservableCollection<EquivalentCostFor≈xistingProduct> _equivalentCostFor≈xistingProductsCollection;
        public virtual ObservableCollection<EquivalentCostFor≈xistingProduct> EquivalentCostFor≈xistingProductsCollection
        {
            get => _equivalentCostFor≈xistingProductsCollection;
            set
            {
                _equivalentCostFor≈xistingProductsCollection = value;
                if(_equivalentCostFor≈xistingProductsCollection != null)
                    _equivalentCostFor≈xistingProductsCollection.CollectionChanged += EquivalentCostFor≈xistingProductsCollectionOnCollectionChanged;
                OnPropertyChanged("EquivalentCostFor≈xistingProductsCollection");
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                        {
                            error = "Õ‡ËÏÂÌÓ‚‡ÌËÂ ‰ÓÎÊÌÓ ·˚Ú¸ ÛÍ‡Á‡ÌÓ";
                        }

                        break;

                    case "Barcode":
                        if (string.IsNullOrEmpty(Barcode))
                        {
                            error = "ÿÚËıÍÓ‰ ‰ÓÎÊÂÌ ·˚Ú¸ ÛÍ‡Á‡Ì";
                        }

                        break;

                    case "IdUnitStorage":
                        if (IdUnitStorage == 0 )
                        {
                            error = "≈‰. ı. ‰ÓÎÊÌ‡ ·˚Ú¸ ÛÍ‡Á‡Ì‡";
                        }

                        break;

                    case "IdWarrantyPeriod":
                        if (IdWarrantyPeriod == 0)
                        {
                            error = "√‡‡ÌÚËÈÌ˚È ÔÂËÓ‰ ‰ÓÎÊÂÌ ·˚Ú¸ ÛÍ‡Á‡Ì";
                        }

                        break;

                    case "IdPriceGroup":
                        if (IdPriceGroup == 0)
                        {
                            error = "÷ÂÌÓ‚‡ˇ „ÛÔÔ‡ ‰ÓÎÊÌ‡ ·˚Ú¸ ÛÍ‡Á‡Ì‡";
                        }

                        break;

                    case "IdCategory":
                        if (IdCategory == 0)
                        {
                            error = " ‡ÚÂ„ÓËˇ ‰ÓÎÊÌ‡ ·˚Ú¸ ÛÍ‡Á‡Ì‡";
                        }

                        break;
                }

                return error;
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title) &&
                                  !string.IsNullOrEmpty(Barcode) &&
                                  IdUnitStorage != 0 && IdWarrantyPeriod != 0 && IdPriceGroup != 0 && IdCategory != 0;

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

            OnPropertyChanged("CountsProductCollection");
        }

        private void CountsProductItemChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CountsProductCollection");
        }

        private void EquivalentCostFor≈xistingProductsCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

            OnPropertyChanged("EquivalentCostFor≈xistingProductsCollection");
        }

        private void EquivalentCostFor≈xistingProductItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("EquivalentCostFor≈xistingProductsCollection");
        }

        #endregion

        public object Clone()
        {
            return new Product
            {
                Barcode = Barcode,
                VendorCode = VendorCode,
                Title = Title,
                Description = Description,
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
                SerialNumbersCollection = new List<SerialNumber>(SerialNumbersCollection.Select(s => (SerialNumber)s.Clone()))
            };
        }
    }
}

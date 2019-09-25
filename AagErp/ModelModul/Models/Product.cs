using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ModelModul.Models
{
    public class Product : ModelBase<Product>
    {
        public Product()
        {
            InvoiceInfosCollection = new ObservableCollection<InvoiceInfo>();
            MovementGoodsInfosCollection = new ObservableCollection<MovementGoodsInfo>();
            PriceProductsCollection = new ObservableCollection<PriceProduct>();
            PropertyProductsCollection = new ObservableCollection<PropertyProduct>();
            SerialNumbersCollection = new ObservableCollection<SerialNumber>();
        }

        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _vendorCode;
        public string VendorCode
        {
            get => _vendorCode;
            set
            {
                _vendorCode = value;
                OnPropertyChanged();
            }
        }

        private string _barcode;
        public virtual string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _idWarrantyPeriod;
        public int IdWarrantyPeriod
        {
            get => _idWarrantyPeriod;
            set
            {
                _idWarrantyPeriod = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int _idCategory;
        public int IdCategory
        {
            get => _idCategory;
            set
            {
                _idCategory = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int _idPriceGroup;
        public int IdPriceGroup
        {
            get => _idPriceGroup;
            set
            {
                _idPriceGroup = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int _idUnitStorage;
        public int IdUnitStorage
        {
            get => _idUnitStorage;
            set
            {
                _idUnitStorage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _keepTrackSerialNumbers;
        public bool KeepTrackSerialNumbers
        {
            get => _keepTrackSerialNumbers;
            set
            {
                _keepTrackSerialNumbers = value;
                OnPropertyChanged();
            }
        }

        private decimal _count;
        public decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private Category _category;
        public virtual Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private PriceGroup _priceGroup;
        public virtual PriceGroup PriceGroup
        {
            get => _priceGroup;
            set
            {
                _priceGroup = value;
                OnPropertyChanged();
            }
        }

        private UnitStorage _unitStorage;
        public virtual UnitStorage UnitStorage
        {
            get => _unitStorage;
            set
            {
                _unitStorage = value;
                OnPropertyChanged();
            }
        }

        private WarrantyPeriod _warrantyPeriod;
        public virtual WarrantyPeriod WarrantyPeriod
        {
            get => _warrantyPeriod;
            set
            {
                _warrantyPeriod = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CountsProduct> _countsProductCollection;
        public ObservableCollection<CountsProduct> CountsProductCollection
        {
            get => _countsProductCollection;
            set
            {
                _countsProductCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<InvoiceInfo> _invoiceInfosCollection;
        public ICollection<InvoiceInfo> InvoiceInfosCollection
        {
            get => _invoiceInfosCollection;
            set
            {
                _invoiceInfosCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfosCollection;
        public ICollection<MovementGoodsInfo> MovementGoodsInfosCollection
        {
            get => _movementGoodsInfosCollection;
            set
            {
                _movementGoodsInfosCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<PriceProduct> _priceProductsCollection;
        public ICollection<PriceProduct> PriceProductsCollection
        {
            get => _priceProductsCollection;
            set
            {
                _priceProductsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<PropertyProduct> _propertyProductsCollection;
        public ICollection<PropertyProduct> PropertyProductsCollection
        {
            get => _propertyProductsCollection;
            set
            {
                _propertyProductsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<SerialNumber> _serialNumbersCollection;
        public ICollection<SerialNumber> SerialNumbersCollection
        {
            get => _serialNumbersCollection;
            set
            {
                if (_serialNumbersCollection is ObservableCollection<SerialNumber>)
                    (_serialNumbersCollection as ObservableCollection<SerialNumber>).CollectionChanged -= OnSerialNumbersCollectionChanged;
                _serialNumbersCollection = value;
                if (_serialNumbersCollection is ObservableCollection<SerialNumber>)
                    (_serialNumbersCollection as ObservableCollection<SerialNumber>).CollectionChanged += OnSerialNumbersCollectionChanged;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EquivalentCostForExistingProduct> _equivalentCostForExistingProductsCollection;
        public ObservableCollection<EquivalentCostForExistingProduct> EquivalentCostForExistingProductsCollection
        {
            get => _equivalentCostForExistingProductsCollection;
            set
            {
                _equivalentCostForExistingProductsCollection = value;
                OnPropertyChanged();
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                        {
                            error = "Наименование должно быть указано";
                        }

                        break;

                    case "Barcode":
                        if (string.IsNullOrEmpty(Barcode))
                        {
                            error = "Штрихкод должен быть указан";
                        }

                        break;

                    case "IdUnitStorage":
                        if (IdUnitStorage <= 0)
                        {
                            error = "Ед. хр. должна быть указана";
                        }

                        break;

                    case "IdWarrantyPeriod":
                        if (IdWarrantyPeriod <= 0)
                        {
                            error = "Гарантийный период должен быть указан";
                        }

                        break;

                    case "IdPriceGroup":
                        if (IdPriceGroup <= 0)
                        {
                            error = "Ценовая группа должна быть указана";
                        }

                        break;

                    case "IdCategory":
                        if (IdCategory <= 0)
                        {
                            error = "Категория должна быть указана";
                        }

                        break;
                }

                return error;
            }
        }

        public override bool IsValid => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Barcode) &&
                                        IdUnitStorage > 0 && IdWarrantyPeriod > 0 && IdPriceGroup > 0 &&
                                        IdCategory > 0 && !HasErrors;

        public override object Clone()
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
                UnitStorage = (UnitStorage)UnitStorage?.Clone(),
                WarrantyPeriod = (WarrantyPeriod)WarrantyPeriod?.Clone(),
                PropertyProductsCollection = PropertyProductsCollection == null ? null : new List<PropertyProduct>(PropertyProductsCollection.Select(s => (PropertyProduct)s.Clone())),
                SerialNumbersCollection = SerialNumbersCollection == null ? null : new List<SerialNumber>(SerialNumbersCollection.Select(s => (SerialNumber)s.Clone()))
            };
        }

        private void OnSerialNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs ea)
        {
            switch (ea.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SerialNumber item in ea.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SerialNumberOnPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumber item in ea.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SerialNumberOnPropertyChanged;
                    }
                    break;
            }
            OnPropertyChanged(nameof(IsValid));
        }

        private void SerialNumberOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(nameof(SerialNumbersCollection));
            OnPropertyChanged(nameof(IsValid));
        }
    }
}

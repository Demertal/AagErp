using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class Product : ModelBase<Product>
    {
        public Product()
        {
            InvoiceInfosCollection = new List<InvoiceInfo>();
            MovementGoodsInfosCollection = new List<MovementGoodsInfo>();
            PriceProductsCollection = new List<PriceProduct>();
            PropertyProductsCollection = new List<PropertyProduct>();
            SerialNumbersCollection = new List<SerialNumber>();
            ValidationRules = new ExpressionSpecification<Product>(
                new ExpressionSpecification<Product>(p => !string.IsNullOrEmpty(p.Title))
                    .And(new ExpressionSpecification<Product>(p => !string.IsNullOrEmpty(p.Barcode)))
                    .And(new ExpressionSpecification<Product>(p => p.IdUnitStorage > 0))
                    .And(new ExpressionSpecification<Product>(p => p.IdWarrantyPeriod > 0))
                    .And(new ExpressionSpecification<Product>(p => p.IdPriceGroup > 0))
                    .And(new ExpressionSpecification<Product>(p => p.IdCategory > 0))
                    .And(new ExpressionSpecification<Product>(p => !p.HasErrors)).IsSatisfiedBy());

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
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("IsValid");
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
        public virtual string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged("Barcode");
                OnPropertyChanged("IsValid");
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
                OnPropertyChanged("IsValid");
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
                OnPropertyChanged("IsValid");
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
                OnPropertyChanged("IsValid");
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
                OnPropertyChanged("IsValid");
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
                _countsProductCollection = value;
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

        private ObservableCollection<EquivalentCostForExistingProduct> _equivalentCostForExistingProductsCollection;
        public virtual ObservableCollection<EquivalentCostForExistingProduct> EquivalentCostForExistingProductsCollection
        {
            get => _equivalentCostForExistingProductsCollection;
            set
            {
                _equivalentCostForExistingProductsCollection = value;
                OnPropertyChanged("EquivalentCostForExistingProductsCollection");
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
                UnitStorage = (UnitStorage) UnitStorage?.Clone(),
                WarrantyPeriod = (WarrantyPeriod) WarrantyPeriod?.Clone(),
                PropertyProductsCollection = PropertyProductsCollection == null ? null : new List<PropertyProduct>(PropertyProductsCollection.Select(s => (PropertyProduct)s.Clone())),
                SerialNumbersCollection = SerialNumbersCollection == null? null : new List<SerialNumber>(SerialNumbersCollection.Select(s => (SerialNumber)s.Clone()))
            };
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

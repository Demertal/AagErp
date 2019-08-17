using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class Product : ModelBase
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
        [Required]
        [StringLength(120)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _vendorCode;
        [StringLength(20)]
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
        [StringLength(13)]
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged("Barcode");
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
    }
}

namespace ModelModul.Models
{
    public class PropertyProduct : ModelBase<PropertyProduct>
    {
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

        private long _idProduct;
        public long IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        private int _idPropertyName;
        public int IdPropertyName
        {
            get => _idPropertyName;
            set
            {
                _idPropertyName = value;
                OnPropertyChanged("IdPropertyName");
            }
        }

        private int? _idPropertyValue;
        public int? IdPropertyValue
        {
            get => _idPropertyValue;
            set
            {
                _idPropertyValue = value;
                OnPropertyChanged("IdPropertyValue");
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }

        private PropertyName _propertyName;
        public virtual PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private PropertyValue _propertyValue;
        public virtual PropertyValue PropertyValue
        {
            get => _propertyValue;
            set
            {
                _propertyValue = value;
                OnPropertyChanged("PropertyValue");
            }
        }

        public override bool IsValid => true;
    }
}

using System.Collections.Generic;

namespace ModelModul.Models
{
    public class PropertyValue : ModelBase
    {
        public PropertyValue()
        {
            PropertyProductsCollection = new HashSet<PropertyProduct>();
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
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
    }
}

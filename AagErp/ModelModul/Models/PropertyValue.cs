using System.Collections.Generic;
using System.Linq;
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
        public virtual string Value
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

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Value":
                        if (string.IsNullOrEmpty(Value))
                        {
                            error = "Значение должно быть указано";
                        }

                        if (PropertyName != null)
                        {
                            if (PropertyName.PropertyValuesCollection.Any(p => p.Value == Value))
                                error = "Такое значение уже есть в этом параметре";
                        }

                        break;
                }

                return error;
            }
        }

        public override bool IsValidate => !string.IsNullOrEmpty(Value);
        public override object Clone()
        {
            return new PropertyValue{Id = Id, IdPropertyName = IdPropertyName, Value = Value};
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace ModelModul.Models
{
    public class PropertyValue : ModelBase<PropertyValue>
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
                OnPropertyChanged();
            }
        }

        private string _value;
        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int _idPropertyName;
        public int IdPropertyName
        {
            get => _idPropertyName;
            set
            {
                _idPropertyName = value;
                OnPropertyChanged();
            }
        }

        private PropertyName _propertyName;
        public virtual PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                OnPropertyChanged();
            }
        }

        private ICollection<PropertyProduct> _propertyProductsCollection;
        public virtual ICollection<PropertyProduct> PropertyProductsCollection
        {
            get => _propertyProductsCollection;
            set
            {
                _propertyProductsCollection = value;
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
                    case "Value":
                        if (string.IsNullOrEmpty(Value))
                        {
                            error = "Значение должно быть указано";
                        }

                        if (PropertyName != null)
                        {
                            if (PropertyName.PropertyValuesCollection.Any(p => p.Value == Value && p.Id != Id))
                                error = "Такое значение уже есть в этом параметре";
                        }

                        break;
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new PropertyValue{Id = Id, IdPropertyName = IdPropertyName, Value = Value};
        }

        public override bool IsValid => !string.IsNullOrEmpty(Value) && PropertyName != null && !HasErrors &&
                                        !PropertyName.PropertyValuesCollection.Any(pp => pp.Value == Value && pp.Id != Id);
    }
}
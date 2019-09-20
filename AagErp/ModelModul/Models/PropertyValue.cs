using System.Collections.Generic;
using System.Linq;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class PropertyValue : ModelBase<PropertyValue>
    {
        public PropertyValue()
        {
            PropertyProductsCollection = new HashSet<PropertyProduct>();
            ValidationRules = new ExpressionSpecification<PropertyValue>(
                new ExpressionSpecification<PropertyValue>(p => !string.IsNullOrEmpty(p.Value))
                    .And(new ExpressionSpecification<PropertyValue>(
                        new ExpressionSpecification<PropertyValue>(p => p.PropertyName == null)
                            .Or(new ExpressionSpecification<PropertyValue>(p =>
                                PropertyName.PropertyValuesCollection.Any(pp => pp.Value == p.Value))).IsSatisfiedBy()))
                    .And(new ExpressionSpecification<PropertyValue>(p => !p.HasErrors)).IsSatisfiedBy());
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
                OnPropertyChanged("IsValid");
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

        public override object Clone()
        {
            return new PropertyValue{Id = Id, IdPropertyName = IdPropertyName, Value = Value};
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

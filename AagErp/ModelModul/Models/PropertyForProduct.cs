namespace ModelModul.Models
{
    public class PropertyForProduct : ModelBase
    {
        private int _propertyNameId;
        public int PropertyNameId
        {
            get => _propertyNameId;
            set
            {
                _propertyNameId = value;
                OnPropertyChanged("PropertyNameId");
            }
        }

        private int _propertyValueId;
        public int PropertyValueId
        {
            get => _propertyValueId;
            set
            {
                _propertyValueId = value;
                OnPropertyChanged("PropertyValueId");
            }
        }

        private PropertyName _propertyName;
        public PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private PropertyValue _propertyValue;
        public PropertyValue PropertyValue
        {
            get => _propertyValue;
            set
            {
                _propertyValue = value;
                OnPropertyChanged("PropertyValue");
            }
        }
    }
}

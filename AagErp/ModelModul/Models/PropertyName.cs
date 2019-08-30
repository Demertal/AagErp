using System.Collections.Generic;

namespace ModelModul.Models
{
    public class PropertyName : ModelBase
    {
        public PropertyName()
        {
            PropertyProductsCollection = new List<PropertyProduct>();
            PropertyValuesCollection = new List<PropertyValue>();
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

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private int? _idCategory;
        public int? IdCategory
        {
            get => _idCategory;
            set
            {
                _idCategory = value;
                OnPropertyChanged("IdCategory");
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

        private ICollection<PropertyValue> _propertyValuesCollection;
        public virtual ICollection<PropertyValue> PropertyValuesCollection
        {
            get => _propertyValuesCollection;
            set
            {
                _propertyValuesCollection = value;
                OnPropertyChanged("PropertyValuesCollection");
            }
        }
    }
}

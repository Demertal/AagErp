using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class PropertyName : ModelBase<PropertyName>
    {
        public PropertyName()
        {
            PropertyProductsCollection = new ObservableCollection<PropertyProduct>();
            PropertyValuesCollection = new ObservableCollection<PropertyValue>();
            ValidationRules = new ExpressionSpecification<PropertyName>(
                new ExpressionSpecification<PropertyName>(p => !string.IsNullOrEmpty(p.Title))
                    .And(new ExpressionSpecification<PropertyName>(p => !p.HasErrors)).IsSatisfiedBy());
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

        private int? _idCategory;
        public virtual int? IdCategory
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
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new PropertyName
            {
                Id = Id,
                IdCategory = IdCategory,
                Title = Title,
                PropertyValuesCollection = PropertyValuesCollection == null
                    ? null
                    : new List<PropertyValue>(PropertyValuesCollection.Select(p => (PropertyValue) p.Clone()))
            };
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Category : ModelBase, ICloneable
    {
        public Category()
        {
            ChildCategoriesCollection = new List<Category>();
            ProductsCollection = new List<Product>();
            PropertyNamesCollection = new List<PropertyName>();
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
                OnPropertyChanged("IsValidate");
            }
        }

        private int? _idParent;
        public int? IdParent
        {
            get => _idParent;
            set
            {
                _idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        private Category _parent;
        public virtual Category Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                OnPropertyChanged("Parent");
            }
        }

        private ICollection<Category> _childCategoriesCollection;
        public virtual ICollection<Category> ChildCategoriesCollection
        {
            get => _childCategoriesCollection;
            set
            {
                _childCategoriesCollection = value;
                OnPropertyChanged("ChildCategoriesCollection");
            }
        }

        private ICollection<Product> _productsCollection;
        public virtual ICollection<Product> ProductsCollection
        {
            get => _productsCollection;
            set
            {
                _productsCollection = value;
                OnPropertyChanged("ProductsCollection");
            }
        }

        private ICollection<PropertyName> _propertyNamesCollection;
        public virtual ICollection<PropertyName> PropertyNamesCollection
        {
            get => _propertyNamesCollection;
            set
            {
                _propertyNamesCollection = value;
                OnPropertyChanged("PropertyNamesCollection");
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title);

        public object Clone()
        {
            return new Category {Id = Id, Title = Title, IdParent = IdParent, Parent = (Category) Parent?.Clone()};
        }
    }
}

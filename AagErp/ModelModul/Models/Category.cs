using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class Category : ModelBase, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            ChildCategories = new List<Category>();
            Products = new List<Product>();
            PropertyNames = new List<PropertyName>();
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

        private ICollection<Category> _childCategories;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> ChildCategories
        {
            get => _childCategories;
            set
            {
                _childCategories = value;
                OnPropertyChanged("ChildCategories");
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

        private ICollection<Product> _products;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }

        private ICollection<PropertyName> _propertyNames;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyName> PropertyNames
        {
            get => _propertyNames;
            set
            {
                _propertyNames = value;
                OnPropertyChanged("PropertyNames");
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title);

        public object Clone()
        {
            return new Category {Id = Id, Title = Title, IdParent = IdParent, Parent = (Category) Parent?.Clone()};
        }
    }
}

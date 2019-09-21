using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModelModul.Models
{
    public class Category : ModelBase<Category>
    {
        public Category()
        {
            ChildCategoriesCollection = new ObservableCollection<Category>();
            ProductsCollection = new ObservableCollection<Product>();
            PropertyNamesCollection = new ObservableCollection<PropertyName>();
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

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int? _idParent;
        public int? IdParent
        {
            get => _idParent;
            set
            {
                _idParent = value;
                OnPropertyChanged();
            }
        }

        private Category _parent;
        public virtual Category Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                OnPropertyChanged();
            }
        }

        private ICollection<Category> _childCategoriesCollection;
        public virtual ICollection<Category> ChildCategoriesCollection
        {
            get => _childCategoriesCollection;
            set
            {
                _childCategoriesCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<Product> _productsCollection;
        public virtual ICollection<Product> ProductsCollection
        {
            get => _productsCollection;
            set
            {
                _productsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<PropertyName> _propertyNamesCollection;
        public virtual ICollection<PropertyName> PropertyNamesCollection
        {
            get => _propertyNamesCollection;
            set
            {
                _propertyNamesCollection = value;
                OnPropertyChanged();
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = String.Empty;

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
            return new Category {Id = Id, Title = Title, IdParent = IdParent};
        }

        public override bool IsValid => !string.IsNullOrEmpty(Title) && !HasErrors;
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class Category : ModelBase<Category>
    {
        public Category()
        {
            ChildCategoriesCollection = new ObservableCollection<Category>();
            ProductsCollection = new ObservableCollection<Product>();
            PropertyNamesCollection = new ObservableCollection<PropertyName>();
            ValidationRules = new ExpressionSpecification<Category>(
                new ExpressionSpecification<Category>(c => !string.IsNullOrEmpty(c.Title))
                    .And(new ExpressionSpecification<Category>(c => !c.HasErrors)).IsSatisfiedBy());
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

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

using System.Collections.Generic;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class UnitStorage : ModelBase<UnitStorage>
    {
        public UnitStorage()
        {
            ProductsCollection = new List<Product>();
            ValidationRules = new ExpressionSpecification<UnitStorage>(
                new ExpressionSpecification<UnitStorage>(u => !string.IsNullOrEmpty(u.Title))
                    .And(new ExpressionSpecification<UnitStorage>(u => !u.HasErrors)).IsSatisfiedBy());
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

        private bool _isWeightGoods;
        public bool IsWeightGoods
        {
            get => _isWeightGoods;
            set
            {
                _isWeightGoods = value;
                OnPropertyChanged("IsWeightGoods");
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
            return new UnitStorage {Id = Id, Title = Title, IsWeightGoods = IsWeightGoods};
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

using System.Collections.Generic;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class PriceGroup : ModelBase<PriceGroup>
    {
        public PriceGroup()
        {
            ProductsCollection = new HashSet<Product>();
            ValidationRules = new ExpressionSpecification<PriceGroup>(
                new ExpressionSpecification<PriceGroup>(p => p.Markup > 0)
                    .And(new ExpressionSpecification<PriceGroup>(p => !p.HasErrors)).IsSatisfiedBy());
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

        private decimal _markup;
        public virtual decimal Markup
        {
            get => _markup;
            set
            {
                _markup = value;
                OnPropertyChanged("Markup");
                OnPropertyChanged("IsValid");
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
                    case "Markup":
                        if (Markup <= 0)
                        {
                            error = "Наценка должна быть больше 0";
                        }

                        break;
                }
                return error;
            }
        }

        public override object Clone()
        {
            return new PriceGroup { Id = Id, Markup = Markup };
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

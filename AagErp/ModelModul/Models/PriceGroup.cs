using System.Collections.Generic;

namespace ModelModul.Models
{
    public class PriceGroup : ModelBase<PriceGroup>
    {
        public PriceGroup()
        {
            ProductsCollection = new HashSet<Product>();
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

        private decimal _markup;
        public virtual decimal Markup
        {
            get => _markup;
            set
            {
                _markup = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
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

        public override bool IsValid => Markup > 0 && !HasErrors;
    }
}
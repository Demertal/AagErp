using System.Collections.Generic;

namespace ModelModul.Models
{
    public class PriceGroup : ModelBase
    {
        public PriceGroup()
        {
            Products = new HashSet<Product>();
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
        public decimal Markup
        {
            get => _markup;
            set
            {
                _markup = value;
                OnPropertyChanged("Markup");
            }
        }

        private ICollection<Product> _products;
        public virtual ICollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }
    }
}

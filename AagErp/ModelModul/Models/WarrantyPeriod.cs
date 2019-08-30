using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class WarrantyPeriod : ModelBase, ICloneable
    {
        public WarrantyPeriod()
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
                OnPropertyChanged("Id");
            }
        }

        private string _period;
        public string Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged("Period");
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

        public object Clone()
        {
            return new WarrantyPeriod {Id = Id, Period = Period};
        }
    }
}

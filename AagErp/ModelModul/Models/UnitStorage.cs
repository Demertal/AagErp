using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class UnitStorage : ModelBase, ICloneable
    {
        public UnitStorage()
        {
            ProductsCollection = new List<Product>();
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

        public object Clone()
        {
            return new UnitStorage {Id = Id, Title = Title, IsWeightGoods = IsWeightGoods};
        }
    }
}

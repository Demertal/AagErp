using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class UnitStorage : ModelBase, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitStorage()
        {
            Products = new List<Product>();
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
        [Required]
        [StringLength(20)]
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

        public object Clone()
        {
            return new UnitStorage {Id = Id, Title = Title, IsWeightGoods = IsWeightGoods};
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelModul.Models
{
    public class MovementGoodsInfo : ModelBase
    {
        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private double _count;
        public double Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        private decimal? _price;
        [Column(TypeName = "money")]
        public decimal? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        private Guid _idReport;
        public Guid IdReport
        {
            get => _idReport;
            set
            {
                _idReport = value;
                OnPropertyChanged("IdReport");
            }
        }

        private long _idProduct;
        public long IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }

        private MovementGoods _movementGoods;
        public virtual MovementGoods MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged("MovementGoods");
            }
        }
    }
}

using System;
using System.Globalization;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class MovementGoodsInfo : ModelBase<MovementGoodsInfo>
    {
        public MovementGoodsInfo()
        {
            ValidationRules = new ExpressionSpecification<MovementGoodsInfo>(
                new ExpressionSpecification<MovementGoodsInfo>(m => m.Price != null && m.Price <= 0)
                    .And(new ExpressionSpecification<MovementGoodsInfo>(m => m.Count > 0))
                    .And(new ExpressionSpecification<MovementGoodsInfo>(
                        new ExpressionSpecification<MovementGoodsInfo>(m => m.Product == null || m.Product.UnitStorage == null).Or(
                            new ExpressionSpecification<MovementGoodsInfo>(m => !m.Product.UnitStorage.IsWeightGoods).Or(
                                new ExpressionSpecification<MovementGoodsInfo>(m => m.Count.ToString(CultureInfo.InvariantCulture).IndexOfAny(new[] { '.', ',' }) == -1))).IsSatisfiedBy()))
                    .And(new ExpressionSpecification<MovementGoodsInfo>(c => !c.HasErrors)).IsSatisfiedBy());
        }

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

        private decimal _count;
        public virtual decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
                OnPropertyChanged("IsValid");
            }
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
                OnPropertyChanged("IsValid");
            }
        }

        private decimal? _equivalentCost;
        public decimal? EquivalentCost
        {
            get => _equivalentCost;
            set
            {
                _equivalentCost = value;
                OnPropertyChanged("EquivalentCost");
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
                OnPropertyChanged("IsValid");
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

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "Price":
                        if (Price != null && Price <= 0)
                        {
                            error = "Цена не может быть меньше и равной 0";
                        }
                        break;
                    case "Count":
                        if (Product?.UnitStorage != null)
                        {
                            if(!Product.UnitStorage.IsWeightGoods && Count.ToString(CultureInfo.InvariantCulture).IndexOfAny(new []{'.', ','}) != -1)
                                error = "Кол-во не может быть дробным";
                        }
                        if (Count <= 0)
                        {
                            error = "Кол-во не может быть меньше и равным 0";
                        }
                        break;
                }
                return error;
            }
        }

        public override object Clone()
        {
            return new MovementGoodsInfo
            {
                Id = Id,
                Count = Count,
                IdProduct = IdProduct,
                IdReport = IdReport,
                Price = Price,
                EquivalentCost = EquivalentCost,
                Product = (Product)Product?.Clone()
            };
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

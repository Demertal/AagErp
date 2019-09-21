using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ModelModul.Models
{
    public class MovementGoodsInfo : ModelBase<MovementGoodsInfo>
    {
        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private decimal _count;
        public virtual decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private decimal? _equivalentCost;
        public decimal? EquivalentCost
        {
            get => _equivalentCost;
            set
            {
                _equivalentCost = value;
                OnPropertyChanged();
            }
        }

        private Guid _idReport;
        public Guid IdReport
        {
            get => _idReport;
            set
            {
                _idReport = value;
                OnPropertyChanged();
            }
        }

        private long _idProduct;
        public long IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                OnPropertyChanged();
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                if(_product != null)
                    _product.PropertyChanged -= ProductOnPropertyChanged;
                _product = value;
                if (_product != null)
                    _product.PropertyChanged += ProductOnPropertyChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private MovementGoods _movementGoods;
        public virtual MovementGoods MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
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

        private void ProductOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(nameof(Product));
            OnPropertyChanged(nameof(IsValid));
        }

        public override bool IsValid => !HasErrors && Price != null && Price > 0 && Count > 0 &&
                                        Product?.UnitStorage != null && Product.SerialNumbersCollection != null &&
                                        (Product.UnitStorage.IsWeightGoods || Count.ToString(CultureInfo.InvariantCulture).IndexOfAny(new[] {'.', ','}) == -1) &&
                                        (!Product.KeepTrackSerialNumbers || Product.SerialNumbersCollection.All(s => s.IsValid));
    }
}
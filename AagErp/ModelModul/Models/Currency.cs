using System.Collections.Generic;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public class Currency : ModelBase<Currency>
    {
        public Currency()
        {
            MovementGoodsCollection = new List<MovementGoods>();
            MovementGoodsEquivalentCollection = new List<MovementGoods>();
            ValidationRules = new ExpressionSpecification<Currency>(
                new ExpressionSpecification<Currency>(c => !string.IsNullOrEmpty(c.Title))
                    .And(new ExpressionSpecification<Currency>(c => c.Cost > 0))
                    .And(new ExpressionSpecification<Currency>(c => !c.HasErrors)).IsSatisfiedBy());
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

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
                OnPropertyChanged("IsValid");
            }
        }

        private bool _isDefault;
        public virtual bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        private ICollection<MovementGoods> _movementGoodsCollection;
        public ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged("MovementGoodsCollection");
            }
        }

        private ICollection<MovementGoods> _movementGoodsEquivalentCollection;
        public ICollection<MovementGoods> MovementGoodsEquivalentCollection
        {
            get => _movementGoodsEquivalentCollection;
            set
            {
                _movementGoodsEquivalentCollection = value;
                OnPropertyChanged("MovementGoodsEquivalentCollection");
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

                    case "Cost":
                        if (Cost <= 0)
                        {
                            error = "Курс должен быть больше 0";
                        }

                        break;
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new Currency {Id = Id, Title = Title, IsDefault = IsDefault, Cost = Cost};
        }

        public override bool IsValid => ValidationRules.IsSatisfiedBy().Compile().Invoke(this);
    }
}

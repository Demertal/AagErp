using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Currency : ModelBase<Currency>
    {
        public Currency()
        {
            MovementGoodsCollection = new List<MovementGoods>();
            MovementGoodsEquivalentCollection = new List<MovementGoods>();
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

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isDefault;
        public virtual bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MovementGoods> _movementGoodsCollection;
        public ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MovementGoods> _movementGoodsEquivalentCollection;
        public ICollection<MovementGoods> MovementGoodsEquivalentCollection
        {
            get => _movementGoodsEquivalentCollection;
            set
            {
                _movementGoodsEquivalentCollection = value;
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

        public override bool IsValid => !string.IsNullOrEmpty(Title) && Cost > 0 && !HasErrors;
    }
}
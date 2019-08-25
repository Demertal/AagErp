using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Currency : ModelBase
    {
        public Currency()
        {
            MovementGoods = new List<MovementGoods>();
            MovementGoodsEquivalent = new List<MovementGoods>();
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

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }

        private bool _isDefault;

        public bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        private ICollection<MovementGoods> _movementGoods;
        public ICollection<MovementGoods> MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged("MovementGoods");
            }
        }

        private ICollection<MovementGoods> _movementGoodsEquivalent;
        public ICollection<MovementGoods> MovementGoodsEquivalent
        {
            get => _movementGoodsEquivalent;
            set
            {
                _movementGoodsEquivalent = value;
                OnPropertyChanged("MovementGoodsEquivalent");
            }
        }
    }
}

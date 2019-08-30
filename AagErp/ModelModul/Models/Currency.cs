using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Currency : ModelBase
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
    }
}

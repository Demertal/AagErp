using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Store : ModelBase
    {
        public Store()
        {
            ArrivalMovementGoodsReports = new List<MovementGoods>();
            DisposalMovementGoodsReports = new List<MovementGoods>();
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

        private ICollection<MovementGoods> _arrivalMovementGoodsReports;
        public virtual ICollection<MovementGoods> ArrivalMovementGoodsReports
        {
            get => _arrivalMovementGoodsReports;
            set
            {
                _arrivalMovementGoodsReports = value;
                OnPropertyChanged("ArrivalMovementGoodsReports");
            }
        }

        private ICollection<MovementGoods> _disposalMovementGoodsReports;
        public virtual ICollection<MovementGoods> DisposalMovementGoodsReports
        {
            get => _disposalMovementGoodsReports;
            set
            {
                _disposalMovementGoodsReports = value;
                OnPropertyChanged("DisposalMovementGoodsReports");
            }
        }
    }
}

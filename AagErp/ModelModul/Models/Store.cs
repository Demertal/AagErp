using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Store : ModelBase
    {
        public Store()
        {
            ArrivalMovementGoodsCollection = new List<MovementGoods>();
            DisposalMovementGoodsCollection = new List<MovementGoods>();
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
            }
        }

        private ICollection<MovementGoods> _arrivalMovementGoodsCollection;
        public virtual ICollection<MovementGoods> ArrivalMovementGoodsCollection
        {
            get => _arrivalMovementGoodsCollection;
            set
            {
                _arrivalMovementGoodsCollection = value;
                OnPropertyChanged("ArrivalMovementGoodsCollection");
            }
        }

        private ICollection<MovementGoods> _disposalMovementGoodsCollection;
        public virtual ICollection<MovementGoods> DisposalMovementGoodsCollection
        {
            get => _disposalMovementGoodsCollection;
            set
            {
                _disposalMovementGoodsCollection = value;
                OnPropertyChanged("DisposalMovementGoodsCollection");
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
                            error = "������������ ������ ���� �������";
                        }

                        break;
                }

                return error;
            }
        }

        public override bool IsValidate => !string.IsNullOrEmpty(Title);

        public override object Clone()
        {
            return new Store { Id = Id, Title = Title};
        }
    }
}

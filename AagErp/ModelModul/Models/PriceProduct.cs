namespace ModelModul.Models
{
    public class PriceProduct : ModelBase
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

        private long _idRevaluation;
        public long IdRevaluation
        {
            get => _idRevaluation;
            set
            {
                _idRevaluation = value;
                OnPropertyChanged("IdRevaluation");
            }
        }

        private decimal _price;
        public virtual decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
                OnPropertyChanged("IsValidate");
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

        private RevaluationProduct _revaluationProduct;
        public virtual RevaluationProduct RevaluationProduct
        {
            get => _revaluationProduct;
            set
            {
                _revaluationProduct = value;
                OnPropertyChanged("RevaluationProduct");
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
                        if (Price <= 0)
                        {
                            error = "���� �� ����� ���� ������ � ������ 0";
                        }
                        break;
                }
                return error;
            }
        }

        public override bool IsValidate => Price > 0;

        public override object Clone()
        {
            return new PriceProduct{Id = Id, IdProduct = IdProduct, IdRevaluation = IdRevaluation, Price = Price};
        }
    }
}

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
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
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
    }
}

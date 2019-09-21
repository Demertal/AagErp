namespace ModelModul.Models
{
    public class InvoiceInfo : ModelBase<InvoiceInfo>
    {
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

        private double _count;
        public double Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _idInvoice;
        public int IdInvoice
        {
            get => _idInvoice;
            set
            {
                _idInvoice = value;
                OnPropertyChanged();
            }
        }

        private Invoice _invoice;
        public virtual Invoice Invoice
        {
            get => _invoice;
            set
            {
                _invoice = value;
                OnPropertyChanged();
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        public override bool IsValid => true;
    }
}

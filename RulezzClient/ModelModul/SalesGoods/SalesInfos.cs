using Prism.Mvvm;

namespace ModelModul
{
    public partial class SalesInfos: BindableBase
    {
        private int _count;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private Products _products;
        public Products Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private SerialNumbers _serialNumbers;
        public SerialNumbers SerialNumbers
        {
            get => _serialNumbers;
            set => SetProperty(ref _serialNumbers, value);
        }
    }
}

namespace ModelModul.Product
{
    public class FinalReportProductViewModel : ProductViewModel
    {
        private int _countPurchase;

        public int CountPurchase
        {
            get => _countPurchase;
            set => SetProperty(ref _countPurchase, value);
        }

        private decimal _finalPurchase;
        public decimal FinalPurchase
        {
            get => _finalPurchase;
            set => SetProperty(ref _finalPurchase, value);
        }

        private int _countSale;

        public int CountSale
        {
            get => _countSale;
            set => SetProperty(ref _countSale, value);
        }

        private decimal _finalSale;
        public decimal FinalSale
        {
            get => _finalSale;
            set => SetProperty(ref _finalSale, value);
        }
    }
}

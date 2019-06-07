using ModelModul;
using Prism.Mvvm;

namespace RevaluationProductsModul.ViewModels
{
    public class RevaluationProductsForRevaluationViewModel: BindableBase
    {
        private RevaluationProductsInfos _revaluationProductsInfo;

        public RevaluationProductsInfos RevaluationProductsInfo
        {
            get => _revaluationProductsInfo;
            set => SetProperty(ref _revaluationProductsInfo, value);
        }

        public Products Product
        {
            get => _revaluationProductsInfo.Products;
            set
            {
                _revaluationProductsInfo.Products = value;
                RaisePropertyChanged();
            }
        }

        public decimal NewSalesPrice
        {
            get => _revaluationProductsInfo.NewSalesPrice;
            set
            {
                _revaluationProductsInfo.NewSalesPrice = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public decimal OldSalesPrice
        {
            get => _revaluationProductsInfo.OldSalesPrice;
            set
            {
                _revaluationProductsInfo.OldSalesPrice = value;
                RaisePropertyChanged();
            }
        }

        private ExchangeRates _exchangeRate;
        public ExchangeRates ExchangeRate
        {
            get => _exchangeRate;
            set
            {
                _exchangeRate = value;
                RaisePropertyChanged();
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                RaisePropertyChanged();
            }
        }

        private ExchangeRates _exchangeRateOld;
        public ExchangeRates ExchangeRateOld
        {
            get => _exchangeRateOld;
            set
            {
                _exchangeRateOld = value;
                RaisePropertyChanged();
            }
        }

        private decimal _purchasePriceOld;
        public decimal PurchasePriceOld
        {
            get => _purchasePriceOld;
            set
            {
                _purchasePriceOld = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidate => NewSalesPrice != 0;
    }
}

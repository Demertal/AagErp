using Prism.Mvvm;

namespace ModelModul
{
    public partial class RevaluationProducts : BindableBase
    {
        private decimal _newSalesPrice;
        public decimal NewSalesPrice
        {
            get => _newSalesPrice;
            set
            {
                _newSalesPrice = value;
                RaisePropertyChanged();
            }
        }
    }
}

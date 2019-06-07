using System;
using Prism.Mvvm;

namespace ModelModul
{
    public partial class RevaluationProductsInfos : BindableBase, ICloneable
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

        public object Clone()
        {
            return new RevaluationProductsInfos
            {
                Id = Id,
                Products = (Products) Products?.Clone(),
                IdProduct = IdProduct,
                NewSalesPrice = NewSalesPrice,
                OldSalesPrice = OldSalesPrice
            };
        }
    }
}

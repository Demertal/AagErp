using System;

namespace ModelModul
{
    public partial class PurchaseInfos: ICloneable
    {
        public object Clone()
        {
            return new PurchaseInfos
            {
                Id = Id,
                Products = (Products) Products?.Clone(),
                Count = Count,
                PurchasePrice = PurchasePrice,
                IdExchangeRate = IdExchangeRate,
                IdProduct = IdProduct,
                ExchangeRates = (ExchangeRates) ExchangeRates?.Clone()
            };
        }
    }
}

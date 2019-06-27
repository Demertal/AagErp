using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ModelModul
{
    public partial class PurchaseInfos: ICloneable
    {
        public object Clone()
        {
            List<SerialNumbers> temp = new List<SerialNumbers>();
            foreach (var serialNumber in SerialNumbers)
            {
                temp.Add((SerialNumbers)serialNumber.Clone());
            }
            return new PurchaseInfos
            {
                Id = Id,
                Products = (Products) Products?.Clone(),
                Count = Count,
                PurchasePrice = PurchasePrice,
                IdExchangeRate = IdExchangeRate,
                IdProduct = IdProduct,
                SerialNumbers = temp,
                ExchangeRates = (ExchangeRates) ExchangeRates?.Clone()
            };
        }
    }
}

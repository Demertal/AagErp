using System;
using System.Collections.Generic;

namespace ModelModul
{
    public partial class PurchaseReports : ICloneable
    {
        public object Clone()
        {
            ICollection<PurchaseInfos> temp = new List<PurchaseInfos>();
            foreach (var purchaseInfo in PurchaseInfos)
            {
                temp.Add((PurchaseInfos)purchaseInfo.Clone());
            }

            return new PurchaseReports
            {
                Id = Id,
                DataOrder = DataOrder,
                IdStore = IdStore,
                IdCounterparty = IdCounterparty,
                PurchaseInfos = temp,
                Stores = Stores,
                Counterparties = Counterparties,
                Course = Course,
                TextInfo = TextInfo
            };
        }
    }
}

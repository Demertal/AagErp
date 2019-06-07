using System;
using System.Collections.Generic;

namespace ModelModul
{
    public partial class SalesReports: ICloneable
    {
        public object Clone()
        {
            List<SalesInfos> temp = new List<SalesInfos>();
            foreach (var salesInfo in SalesInfos)
            {
                temp.Add((SalesInfos)salesInfo.Clone());
            }
            return  new SalesReports
            {
                Id = Id,
                DataSales = DataSales,
                IdStore = IdStore,
                Stores = (Stores)Stores?.Clone(),
                IdCounterparty = IdCounterparty,
                Counterparties = (Counterparties)Counterparties?.Clone(),
                SalesInfos = temp
            };
        }
    }
}

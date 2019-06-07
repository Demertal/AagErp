using System;
using System.Collections.Generic;

namespace ModelModul
{
    public partial class RevaluationProductsReports: ICloneable
    {
        public object Clone()
        {
            ICollection<RevaluationProductsInfos> temp = new List<RevaluationProductsInfos>();
            foreach (var revaluationProductsInfo in RevaluationProductsInfos)
            {
                temp.Add((RevaluationProductsInfos)revaluationProductsInfo.Clone());
            }

            return new RevaluationProductsReports
            {
                Id = Id,
                DataRevaluation = DataRevaluation,
                RevaluationProductsInfos = temp
            };
        }
    }
}

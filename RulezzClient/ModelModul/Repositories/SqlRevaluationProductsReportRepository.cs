using System;
using System.Threading.Tasks;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlRevaluationProductsReportRepository: SqlRepository<RevaluationProducts>
    {
        public override Task UpdateAsync(RevaluationProducts obj)
        {
            throw new NotImplementedException();
        }

        //public override Task DeleteAsync(RevaluationProducts item)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

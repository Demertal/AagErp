using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlProfitStatementRepository :SqlRepository<ProfitStatement>
    {
        public async Task<IEnumerable<ProfitStatement>> GetProfitStatement(DateTime from, DateTime to)
        {
            return await Db.ProfitStatement.FromSql("SELECT * FROM dbo.getReport({0}, {1})", from, to).Include(ps => ps.Product).ThenInclude(p => p.UnitStorage).ToListAsyncEF();
        }
    }
}

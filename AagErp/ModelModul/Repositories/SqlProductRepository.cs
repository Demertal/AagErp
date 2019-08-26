using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{
    public class SqlProductRepository : SqlRepository<Product>
    {
        public override async Task<IEnumerable<Product>> GetListAsync(ISpecification<Product> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<Product, Object>>[] include)
        {
            var query = Db.Products.FromSql("select * from productsWithCountAndPrice").AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy());
            if (order != null) query = query.OrderUsingSortExpression(order);
            if (skip != 0) query = query.Skip(skip);
            if (take != -1) query = query.Take(take);
            if (include != null) query = query.ToLoad(include);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<List<CountsProduct>> GetCountsProduct(long itemId)
        {
            return await Db.CountsProducts
                .FromSql("select * from dbo.getCountProduct ({0})", itemId).Include(c => c.Store).AsNoTracking().ToListAsync();
        }

        public async Task<List<EquivalentCostForЕxistingProduct>> GetEquivalentCostsForЕxistingProduct(long itemId)
        {
            return await Db.EquivalentCostForЕxistingProducts
                .FromSql("select * from dbo.getEquivalentCostForЕxistingProduct ({0})", itemId).Include(c => c.EquivalentCurrency).AsNoTracking().ToListAsync();
        }

        public async Task<decimal> GetCurrentPrice(long itemId)
        {
            return await Db.Products.Select(p => AutomationAccountingGoodsContext.GetCurrentPrice(itemId))
                .FirstOrDefaultAsync();
        }

        public override async Task UpdateAsync(Product item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    Db.Products.Update(item);
                    Db.Entry(item).Property(p => p.Count).IsModified = false;
                    Db.Entry(item).Property(p => p.Price).IsModified = false;
                    await Db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

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
        public async Task<IEnumerable<Product>> GetProductsWithCountAndPrice(ISpecification<ProductWithCountAndPrice> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<ProductWithCountAndPrice, Object>>[] include)
        {
            var query = Db.ProductWithCountAndPrice.AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy()).AsQueryable();
            if (order != null) query = query.OrderUsingSortExpression(order).AsQueryable();
            if (skip != 0) query = query.Skip(skip).AsQueryable();
            if (take != -1) query = query.Take(take).AsQueryable();
            if (include != null) query = query.ToLoad(include).AsQueryable();
            return await query.Select(p => (Product)p).ToListAsync();
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
    }
}

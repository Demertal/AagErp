using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{
    public class HierarchyCategoriesCte
    {
        public int? Id { get; set; }

        public int? IdParent { get; set; }
    }

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
            return await EntityFrameworkQueryableExtensions.ToListAsync(query.Select(p => (Product)p));
        }

        public async Task<IEnumerable<CountsProduct>> GetCountsProduct(long itemId)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(Db.CountsProducts
                    .FromSql("select * from dbo.getCountProduct ({0})", itemId).Include(c => c.Store).AsNoTracking());
        }

        public async Task<IEnumerable<PropertyProduct>> GetPropertyForProduct(Product item)
        {
            Configuration.Linq.AllowMultipleQuery = true;
            var hierarchyCategoryCte = Db.CreateLinqToDbContext().GetCte<HierarchyCategoriesCte>(hierarchyCategory =>
            {
                return
                    (
                        from c in Db.MoneyTransferTypes
                        select new HierarchyCategoriesCte
                        {
                            Id = null,
                            IdParent = null
                        }
                     ).Take(1).Concat
                    (
                        from c in Db.Categories
                        where c.Id == item.IdCategory
                        select new HierarchyCategoriesCte
                        {
                            Id = c.Id,
                            IdParent = c.IdParent,
                        }
                    )
                    .Concat
                    (
                        from c in Db.Categories
                        from hc in hierarchyCategory
                            .InnerJoin(hc => hc.IdParent != null && c.Id == hc.IdParent)
                        select new HierarchyCategoriesCte
                        {
                            Id = c.Id,
                            IdParent = c.IdParent
                        }
                    );
            });
            var result = Db.PropertyNames.Include(pm => pm.PropertyValuesCollection)
                .SelectMany(
                    pm => hierarchyCategoryCte.InnerJoin(hc =>
                        pm.IdCategory == null && hc.Id == null || pm.IdCategory == hc.Id), (pm, hc) => new {pm, hc})
                .SelectMany(
                    @t => Db.PropertyProducts.LeftJoin(pp => pp.IdPropertyName == @t.pm.Id && pp.IdProduct == item.Id),
                    (@t, pp) => new PropertyProduct
                    {
                        Id = pp == null ? 0 : pp.Id,
                        PropertyName = t.pm,
                        IdPropertyName = @t.pm.Id,
                        IdPropertyValue = pp == null ? null : pp.IdPropertyValue
                    });

            return await result.ToListAsyncLinqToDB();
        }

        public async Task<IEnumerable<EquivalentCostForЕxistingProduct>> GetEquivalentCostsForЕxistingProduct(long itemId)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(Db.EquivalentCostForЕxistingProducts
                    .FromSql("select * from dbo.getEquivalentCostForЕxistingProduct ({0})", itemId).Include(c => c.EquivalentCurrency).AsNoTracking());
        }

        public async Task<decimal> GetCurrentPrice(long itemId)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(Db.Products.Select(p => AutomationAccountingGoodsContext.GetCurrentPrice(itemId)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Product>> GetProductsWithCountAndPrice(ISpecification<Product> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<Product, Object>>[] include)
        {
            var query = from p in Db.Products
                select p;

            if (where != null) query = query.Where(where.IsSatisfiedBy()).AsQueryable();
            if (order != null) query = query.OrderUsingSortExpression(order).AsQueryable();
            if (skip != 0) query = query.Skip(skip).AsQueryable();
            if (take != -1) query = query.Take(take).AsQueryable();
            if (include != null) query = query.ToLoad(include).AsQueryable();
            query =  from q in query
                     from pwcap in Db.ProductWithCountAndPrice
                            .InnerJoin(pwcap => q.Id == pwcap.Id)
                     select new Product
                     {
                         Title = q.Title,
                         Barcode = q.Barcode,
                         Id = q.Id,
                         Description = q.Description,
                         IdCategory = q.IdCategory,
                         IdPriceGroup = q.IdPriceGroup,
                         IdUnitStorage = q.IdUnitStorage,
                         IdWarrantyPeriod = q.IdWarrantyPeriod,
                         KeepTrackSerialNumbers = q.KeepTrackSerialNumbers,
                         Price = pwcap.Price,
                         Count = pwcap.Count
                     };
            return await query.ToListAsyncLinqToDB();
        }

        public async Task<IEnumerable<CountsProduct>> GetCountsProduct(long itemId)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(Db.CountsProducts
                    .FromSql("select * from dbo.getCountProduct ({0})", itemId).Include(c => c.Store).AsNoTracking());
        }

        public async Task<IEnumerable<PropertyProduct>> GetPropertyForProduct(Product item)
        {
            return await GetPropertyForProduct(item.IdCategory, item.Id);
        }

        public async Task<IEnumerable<PropertyProduct>> GetPropertyForProduct(int? idCategory, long id = 0)
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
                        where c.Id == idCategory
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
                        pm.IdCategory == null && hc.Id == null || pm.IdCategory == hc.Id), (pm, hc) => new { pm, hc })
                .SelectMany(
                    @t => Db.PropertyProducts.LeftJoin(pp => pp.IdPropertyName == @t.pm.Id && pp.IdProduct == id),
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

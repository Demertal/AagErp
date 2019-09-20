using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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
        public async Task<IEnumerable<Product>> GetProductsWithCountAndPrice(CancellationToken cts = new CancellationToken(), ISpecification<Product> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<Product, Object>>[] include)
        {
            return await Task.Run(() =>
            {
                var query = from p in Db.Products
                    select p;

                cts.ThrowIfCancellationRequested();

                if (where != null) query = query.Where(where.IsSatisfiedBy()).AsQueryable();
                if (order != null) query = query.OrderUsingSortExpression(order).AsQueryable();
                if (skip != 0) query = query.Skip(skip).AsQueryable();
                if (take != -1) query = query.Take(take).AsQueryable();
                if (include != null) query = query.ToLoad(include).AsQueryable();

                cts.ThrowIfCancellationRequested();

                query = from q in query
                    join pwcap in Db.ProductWithCountAndPrice on q.Id equals pwcap.Id
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
                        Count = pwcap.Count,
                        Category = q.Category,
                        PriceGroup = q.PriceGroup,
                        UnitStorage = q.UnitStorage,
                        VendorCode = q.VendorCode,
                        WarrantyPeriod = q.WarrantyPeriod,
                        PropertyProductsCollection = q.PropertyProductsCollection,
                        PriceProductsCollection = q.PriceProductsCollection,
                        MovementGoodsInfosCollection = q.MovementGoodsInfosCollection,
                        InvoiceInfosCollection = q.InvoiceInfosCollection,
                        SerialNumbersCollection = q.SerialNumbersCollection,
                        CountsProductCollection = q.CountsProductCollection,
                        EquivalentCostForExistingProductsCollection = q.EquivalentCostForExistingProductsCollection
                    };

                cts.ThrowIfCancellationRequested();
                return EntityFrameworkQueryableExtensions.ToListAsync(query, cts);
            }, cts);
        }

        public async Task<IEnumerable<CountsProduct>> GetCountsProduct(long itemId, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return EntityFrameworkQueryableExtensions.ToListAsync(Db.CountsProducts
                        .FromSql("select * from dbo.getCountProduct ({0})", itemId).Include(c => c.Store)
                        .AsNoTracking(),
                    cts);
            }, cts);
        }

        public async Task<IEnumerable<PropertyProduct>> GetPropertyForProduct(Product item, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return GetPropertyForProduct(item.IdCategory, item.Id, cts);
            }, cts);
        }

        public async Task<IEnumerable<PropertyProduct>> GetPropertyForProduct(int? idCategory, long id = 0, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
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
                cts.ThrowIfCancellationRequested();
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
                cts.ThrowIfCancellationRequested();
                return result.ToListAsyncLinqToDB(cts);
            }, cts);
        }

        public async Task<IEnumerable<EquivalentCostForExistingProduct>> GetEquivalentCostsForЕxistingProduct(long itemId, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return EntityFrameworkQueryableExtensions.ToListAsync(
                    Db.EquivalentCostForЕxistingProducts
                        .FromSql("select * from dbo.getEquivalentCostForЕxistingProduct ({0})", itemId).AsNoTracking(),
                    cts);
            }, cts);
        }

        public async Task<decimal> GetCurrentPrice(long itemId, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                    Db.MovmentGoodTypes.Select(p => AutomationAccountingGoodsContext.GetCurrentPrice(itemId)), cts);
            }, cts);
        }
    }
}

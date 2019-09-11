using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{


    public abstract class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AutomationAccountingGoodsContext Db;

        protected SqlRepository()
        {
            Db = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString);
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1)
        {
            var query = Db.Set<TEntity>().AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy());
            if (order != null) query = query.OrderUsingSortExpression(order);
            if (skip != 0) query = query.Skip(skip);
            if (take != -1) query = query.Take(take);
            return await query.CountAsync();
        }

        public async Task<bool> AnyAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1)
        {
            var query = Db.Set<TEntity>().AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy());
            if (order != null) query = query.OrderUsingSortExpression(order);
            if (skip != 0) query = query.Skip(skip);
            if (take != -1) query = query.Take(take);
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<TEntity, Object>>[] include)
        {
            var query = Db.Set<TEntity>().AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy());
            if (order != null) query = query.OrderUsingSortExpression(order);
            if (skip != 0) query = query.Skip(skip);
            if (take != -1) query = query.Take(take);
            if (include != null) query = query.ToLoad(include);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetItemAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0)
        {
            var query = Db.Set<TEntity>().AsQueryable();
            if (where != null) query = query.Where(where.IsSatisfiedBy());
            if (order != null) query = query.OrderUsingSortExpression(order);
            if (skip != 0) query = query.Skip(skip);
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetItemAsync(int id)
        {
            return await Db.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetItemAsync(long id)
        {
            return await Db.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> GetItemAsync(Guid id)
        {
            return await Db.Set<TEntity>().FindAsync(id);
        }

        public async Task CreateAsync(TEntity item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    Db.Set<TEntity>().Add(item);
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

        public async Task UpdateAsync(TEntity item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    Db.Set<TEntity>().Update(item);
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

        public async Task DeleteAsync(TEntity item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    //var counterparty = await GetItemAsync(item.Id);
                    //if (counterparty == null) throw new Exception("Контрагент не найден");
                    Db.Entry(item).State = EntityState.Deleted;
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

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

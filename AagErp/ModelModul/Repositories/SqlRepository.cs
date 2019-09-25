using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{


    public abstract class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly AutomationAccountingGoodsContext Db;

        protected SqlRepository()
        {
            Db = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString);
        }

        public async Task<int> GetCountAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1)
        {
            return await Task.Run(() =>
            {
                var query = Db.Set<TEntity>().AsQueryable();
                if (where != null) query = query.Where(where.IsSatisfiedBy());
                if (order != null) query = query.OrderUsingSortExpression(order);
                if (skip != 0) query = query.Skip(skip);
                if (take != -1) query = query.Take(take);
                cts.ThrowIfCancellationRequested();
                return query.CountAsync(cts);
            }, cts);
        }

        public async Task<bool> AnyAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1)
        {
            return await Task.Run(() =>
            {
                var query = Db.Set<TEntity>().AsQueryable();
                if (where != null) query = query.Where(where.IsSatisfiedBy());
                if (order != null) query = query.OrderUsingSortExpression(order);
                if (skip != 0) query = query.Skip(skip);
                if (take != -1) query = query.Take(take);
                cts.ThrowIfCancellationRequested();
                return query.AnyAsync(cts);
            }, cts);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params (Expression<Func<TEntity, Object>> include, Expression<Func<Object, Object>>[] thenInclude)[] include)
        {
            return await Task.Run(() =>
            {
                var query = Db.Set<TEntity>().AsQueryable();
                if (where != null) query = query.Where(where.IsSatisfiedBy());
                if (order != null) query = query.OrderUsingSortExpression(order);
                if (skip != 0) query = query.Skip(skip);
                if (take != -1) query = query.Take(take);
                if (include != null) query = query.ToLoad(include);
                cts.ThrowIfCancellationRequested();
                return query.AsNoTracking().ToListAsync(cts);
            }, cts);
        }

        public async Task<TEntity> GetItemAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0)
        {
            return await Task.Run(() =>
            {
                var query = Db.Set<TEntity>().AsQueryable();
                if (where != null) query = query.Where(where.IsSatisfiedBy());
                if (order != null) query = query.OrderUsingSortExpression(order);
                if (skip != 0) query = query.Skip(skip);
                cts.ThrowIfCancellationRequested();
                return query.AsNoTracking().FirstOrDefaultAsync(cts);
            }, cts);
        }

        public async Task<TEntity> GetItemAsync(int id, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return Db.Set<TEntity>().FindAsync(id, cts);
            }, cts);
        }

        public async Task<TEntity> GetItemAsync(long id, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return Db.Set<TEntity>().FindAsync(id, cts);
            }, cts);
        }

        public virtual async Task<TEntity> GetItemAsync(Guid id, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return Db.Set<TEntity>().FindAsync(id, cts);
            }, cts);
        }

        public async Task CreateAsync(TEntity item, CancellationToken cts = new CancellationToken())
        {
            if(Db.Database.CurrentTransaction == null)
            {
                using (var transaction = Db.Database.BeginTransaction())
                {
                    try
                    {
                        Db.Set<TEntity>().Add(item);
                        await Db.SaveChangesAsync(cts);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                Db.Set<TEntity>().Add(item);
                await Db.SaveChangesAsync(cts);
            }
        }

        public async Task UpdateAsync(TEntity item, CancellationToken cts = new CancellationToken())
        {
            if (Db.Database.CurrentTransaction == null)
            {
                using (var transaction = Db.Database.BeginTransaction())
                {
                    try
                    {
                        Db.Set<TEntity>().Update(item);
                        await Db.SaveChangesAsync(cts);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                Db.Set<TEntity>().Update(item);
                await Db.SaveChangesAsync(cts);
            }
        }

        public async Task DeleteAsync(TEntity item, CancellationToken cts = new CancellationToken())
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    Db.Entry(item).State = EntityState.Deleted;
                    await Db.SaveChangesAsync(cts);
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

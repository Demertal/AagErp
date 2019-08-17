using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlStoreRepository : SqlRepository<Store>
    {
        public override async Task UpdateAsync(Store item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var store = await GetItemAsync(item.Id);
                    if (store == null) throw new Exception("Склад не найден");
                    store.Title = item.Title;
                    Db.Entry(store).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(Store item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var store = await GetItemAsync(item.Id);
        //            if (store == null) throw new Exception("Склад не найден");
        //            Db.Entry(store).State = EntityState.Deleted;
        //            await Db.SaveChangesAsync();
        //            transaction.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Rollback();
        //            throw;
        //        }
        //    }
        //}
    }
}

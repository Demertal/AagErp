using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlWarrantyPeriodRepository : SqlRepository<WarrantyPeriod>
    {
        //public override async Task UpdateAsync(WarrantyPeriod item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var warrantyPeriod = await GetItemAsync(item.Id);
        //            if (warrantyPeriod == null) throw new Exception("Гарантйный период не найден");
        //            warrantyPeriod.Period = warrantyPeriod.Period;
        //            Db.Entry(Db).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(WarrantyPeriod item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var warrantyPeriod = await GetItemAsync(item.Id);
        //            if (warrantyPeriod == null) throw new Exception("Гарантйный период не найден");
        //            Db.Entry(warrantyPeriod).State = EntityState.Deleted;
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

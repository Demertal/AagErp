using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlUnitStorageRepository : SqlRepository<UnitStorage>
    {
        //public override async Task UpdateAsync(UnitStorage item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var unitStorage = await GetItemAsync(item.Id);
        //            if (unitStorage == null) throw new Exception("Единица хранения не найдена");
        //            unitStorage.Title = item.Title;
        //            unitStorage.IsWeightGoods = item.IsWeightGoods;
        //            Db.Entry(unitStorage).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(UnitStorage item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var unitStorage = await GetItemAsync(item.Id);
        //            if (unitStorage == null) throw new Exception("Единица хранения не найдена");
        //            Db.Entry(unitStorage).State = EntityState.Deleted;
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

using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlWarrantyRepository : SqlRepository<Warranty>
    {
        //public override async Task UpdateAsync(Warranty item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var warranty = await GetItemAsync(item.Id);
        //            if (warranty == null) throw new Exception("Гарантия не найдена");
        //            warranty.Malfunction = item.Malfunction;
        //            warranty.Info = item.Info;
        //            warranty.DateDeparture = item.DateDeparture;
        //            warranty.DateIssue = item.DateIssue;
        //            warranty.DateReceipt = item.DateReceipt;
        //            Db.Entry(warranty).State = EntityState.Modified;
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

        //public override Task DeleteAsync(Warranty item)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

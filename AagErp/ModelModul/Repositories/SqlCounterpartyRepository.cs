using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlCounterpartyRepository: SqlRepository<Counterparty>
    {
        //public override async Task UpdateAsync(Counterparty item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var counterparty = await GetItemAsync(item.Id);
        //            if (counterparty == null) throw new Exception("Контрагент не найден");
        //            counterparty.Title = item.Title;
        //            counterparty.Address = item.Address;
        //            counterparty.ContactPerson = item.ContactPerson;
        //            counterparty.ContactPhone = item.ContactPhone;
        //            counterparty.Props = item.Props;
        //            Db.Entry(counterparty).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(Counterparty item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var counterparty = await GetItemAsync(item.Id);
        //            if (counterparty == null) throw new Exception("Контрагент не найден");
        //            Db.Entry(counterparty).State = EntityState.Deleted;
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

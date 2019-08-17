using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlExchangeRateRepository: SqlRepository<Currency>
    {
        public override async Task UpdateAsync(Currency item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var exchangeRate = await GetItemAsync(item.Id);
                    if (exchangeRate == null) throw new Exception("Валюта не найдена");
                    exchangeRate.Title = item.Title;
                    exchangeRate.Cost = item.Cost;
                    exchangeRate.IsDefault = item.IsDefault;
                    Db.Entry(exchangeRate).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(Currency item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var exchangeRate = await GetItemAsync(item.Id);
        //            if (exchangeRate == null) throw new Exception("Валюта не найдена");
        //            Db.Entry(exchangeRate).State = EntityState.Deleted;
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

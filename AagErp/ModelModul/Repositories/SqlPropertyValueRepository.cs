using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlPropertyValueRepository : SqlRepository<PropertyValue>
    {
        //public override async Task UpdateAsync(PropertyValue item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var propertyValue = await GetItemAsync(item.Id);
        //            if (propertyValue == null) throw new Exception("Значение не найдено");

        //            propertyValue.Value = propertyValue.Value;
        //            Db.Entry(propertyValue).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(PropertyValue item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var propertyValue = await GetItemAsync(item.Id);
        //            if (propertyValue == null) throw new Exception("Значение не найдено");
        //            Db.Entry(propertyValue).State = EntityState.Deleted;
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

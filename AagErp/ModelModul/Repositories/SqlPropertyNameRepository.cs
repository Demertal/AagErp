using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlPropertyNameRepository : SqlRepository<PropertyName>
    {
       //public override async Task UpdateAsync(PropertyName item)
       // {
       //     using (var transaction = Db.Database.BeginTransaction())
       //     {
       //         try
       //         {
       //             var propertyName = await GetItemAsync(item.Id);
       //             if (propertyName == null) throw new Exception("Свойство не найдено");
       //             propertyName.Title = item.Title;
       //             Db.Entry(propertyName).State = EntityState.Modified;
       //             await Db.SaveChangesAsync();
       //             transaction.Commit();
       //         }
       //         catch (Exception)
       //         {
       //             transaction.Rollback();
       //             throw;
       //         }
       //     }
       // }

        //public override async Task DeleteAsync(PropertyName item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var propertyName = await GetItemAsync(item.Id);
        //            if (propertyName == null) throw new Exception("Свойство не найдено");
        //            Db.Entry(propertyName).State = EntityState.Deleted;
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

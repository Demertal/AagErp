using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlCategoryRepository : SqlRepository<Category>
    {
        public override async Task UpdateAsync(Category item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var group = await GetItemAsync(item.Id);
                    if (group == null) throw new Exception("Группа не найдена");
                    group.Title = item.Title;
                    Db.Entry(group).State = EntityState.Modified;
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

        //public override async Task DeleteAsync(Parent item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var category = await GetItemAsync(item.Id);
        //            if (category == null) throw new Exception("Группа не найдена");
        //            Db.Entry(category).State = EntityState.Deleted;
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

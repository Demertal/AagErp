using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlPropertyProductRepository : SqlRepository<PropertyProduct>
    {
        //public void Update(List<PropertyProduct> objList)
        //{
        //    using (AutomationAccountingGoodsContext db =
        //        new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
        //    {
        //        using (var transaction = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (var obj in objList)
        //                {
        //                    var property = db.PropertyProducts
        //                        .Find(obj.Id);
        //                    if (property == null) throw new Exception("Изменить не получилось");

        //                    property.IdPropertyValue = obj.IdPropertyValue;
        //                    db.Entry(property).State = EntityState.Modified;
        //                }

        //                db.SaveChanges();
        //                transaction.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}

        //public override async Task UpdateAsync(PropertyProduct item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var propertyProduct = await GetItemAsync((int)item.Id);
        //            if (propertyProduct == null) throw new Exception("Знвчение свойсва товара не найдено");

        //            propertyProduct.IdPropertyValue = item.IdPropertyValue;
        //            Db.Entry(propertyProduct).State = EntityState.Modified;
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

        //public override Task DeleteAsync(PropertyProduct item)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

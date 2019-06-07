using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.PropertyProduct
{
    public class DbSetPropertyProducts : AutomationAccountingGoodsEntities, IDbSetModel<PropertyProducts>
    {
        public async Task<ObservableCollection<PropertyProducts>> LoadAsync(int idProduct)
        {
            await PropertyProducts.Where(obj => obj.IdProduct == idProduct).Include(obj => obj.PropertyNames).Include(obj => obj.PropertyNames.PropertyValues).LoadAsync();
            return PropertyProducts.Local;
        }

        public Task AddAsync(PropertyProducts obj)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PropertyProducts obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var property = PropertyProducts.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                    property.IdPropertyValue = obj.IdPropertyValue;
                    Entry(property).State = EntityState.Modified;
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(List<PropertyProducts> objList)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in objList)
                    {
                        var property = PropertyProducts.Find(obj.Id);
                        if (property == null) throw new Exception("Изменить не получилось");

                        property.IdPropertyValue = obj.IdPropertyValue;
                        Entry(property).State = EntityState.Modified;
                    }
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

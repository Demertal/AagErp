using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.PropertyValue
{
    public class DbSetPropertyValue : AutomationAccountingGoodsEntities, IDbSetModel<PropertyValues>
    {
        public async Task<ObservableCollection<PropertyValues>> LoadAsync(int idPropertyName)
        {
            await PropertyValues.Where(obj => obj.IdPropertyName == idPropertyName).LoadAsync();
            return PropertyValues.Local;
        }

        public async Task AddAsync(PropertyValues obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    if (obj.PropertyNames != null)
                    {
                        obj.IdPropertyName = obj.PropertyNames.Id;
                        obj.PropertyNames = null;
                    }

                    PropertyValues.Add(obj);
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

        public async Task UpdateAsync(PropertyValues obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var property = PropertyValues.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                       property.Value = obj.Value;
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

        public async Task DeleteAsync(int objId)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var property = PropertyValues.Find(objId);
                    Entry(property).State = EntityState.Deleted;
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

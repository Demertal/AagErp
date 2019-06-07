using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriods : AutomationAccountingGoodsEntities, IDbSetModel<WarrantyPeriods>
    {
        public async Task<ObservableCollection<WarrantyPeriods>> LoadAsync()
        {
            await WarrantyPeriods.LoadAsync();
            return WarrantyPeriods.Local;
        }

        public async Task AddAsync(WarrantyPeriods obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    WarrantyPeriods.Add(obj);
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

        public async Task UpdateAsync(WarrantyPeriods obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var unit = WarrantyPeriods.Find(obj.Id);
                    if (unit == null) throw new Exception("Изменить не получилось");
                    if (unit.Period == "Нет") throw new Exception("Нельзя изменять гарантийный период: \"Нет\"");
                    unit.Period = obj.Period;
                    Entry(unit).State = EntityState.Modified;
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
                    var unit = WarrantyPeriods.Find(objId);
                    if (unit == null) throw new Exception("Удалить не получилось");
                    if (unit.Period == "Нет") throw new Exception("Нельзя удалять гарантийный период: \"Нет\"");
                    Entry(unit).State = EntityState.Deleted;
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

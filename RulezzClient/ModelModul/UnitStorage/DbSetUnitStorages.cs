using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStorages : AutomationAccountingGoodsEntities, IDbSetModel<UnitStorages>
    {
        public async Task<ObservableCollection<UnitStorages>> LoadAsync()
        {
            await UnitStorages.LoadAsync();
            return UnitStorages.Local;
        }

        public async Task AddAsync(UnitStorages obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    UnitStorages.Add(obj);
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

        public async Task UpdateAsync(UnitStorages obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var unit = UnitStorages.Find(obj.Id);
                    if (unit == null) throw new Exception("Изменить не получилось");
                    if (unit.Title == "шт") throw new Exception("Нельзя изменять ед. хр.: \"шт\"");
                    unit.Title = obj.Title;
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
                    var unit = UnitStorages.Find(objId);
                    if (unit == null) throw new Exception("Удалить не получилось");
                    if (unit.Title == "шт") throw new Exception("Нельзя удалять ед. хр.: \"шт\"");
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

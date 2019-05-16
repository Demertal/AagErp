using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriods : DbSetModel<WarrantyPeriods>
    {
        public async Task LoadAsync()
        {
            using (StoreEntities db = new StoreEntities())
            {
                await db.WarrantyPeriods.LoadAsync();
                List = db.WarrantyPeriods.Local;
            }
        }

        public override async Task AddAsync(WarrantyPeriods obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.WarrantyPeriods.Add(obj);
                        await db.SaveChangesAsync();
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

        public override async Task UpdateAsync(WarrantyPeriods obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var unit = db.WarrantyPeriods.Find(obj.Id);
                        if (unit == null) throw new Exception("Изменить не получилось");
                        if (unit.Period == "Нет") throw new Exception("Нельзя изменять гарантийный период: \"Нет\"");
                        unit.Period = obj.Period;
                        db.Entry(unit).State = EntityState.Modified;
                        await db.SaveChangesAsync();
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

        public override async Task DeleteAsync(int objId)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var unit = db.WarrantyPeriods.Find(objId);
                        if (unit == null) throw new Exception("Удалить не получилось");
                        if (unit.Period == "Нет") throw new Exception("Нельзя удалять гарантийный период: \"Нет\"");
                        db.Entry(unit).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
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
}

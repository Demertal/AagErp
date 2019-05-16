using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStorages : DbSetModel<UnitStorages>
    {
        public async Task LoadAsync()
        {
            using (StoreEntities db = new StoreEntities())
            {
                await db.UnitStorages.LoadAsync();
                List = db.UnitStorages.Local;
            }
        }

        public override async Task AddAsync(UnitStorages obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.UnitStorages.Add(obj);
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

        public override async Task UpdateAsync(UnitStorages obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var unit = db.UnitStorages.Find(obj.Id);
                        if (unit == null) throw new Exception("Изменить не получилось");
                        if (unit.Title == "шт") throw new Exception("Нельзя изменять ед. хр.: \"шт\"");
                        unit.Title = obj.Title;
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
                        var unit = db.UnitStorages.Find(objId);
                        if (unit == null) throw new Exception("Удалить не получилось");
                        if (unit.Title == "шт") throw new Exception("Нельзя удалять ед. хр.: \"шт\"");
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

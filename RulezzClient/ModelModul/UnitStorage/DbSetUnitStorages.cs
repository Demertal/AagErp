using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStorages : IDbSetModel<UnitStorages>
    {
        public ObservableCollection<UnitStorages> Load()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<UnitStorages>(db.UnitStorages);
            }
        }

        public void Add(UnitStorages obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.UnitStorages.Add(obj);
                        db.SaveChanges();
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

        public void Update(UnitStorages obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
                        db.SaveChanges();
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

        public void Delete(int objId)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var unit = db.UnitStorages.Find(objId);
                        if (unit == null) throw new Exception("Удалить не получилось");
                        if (unit.Title == "шт") throw new Exception("Нельзя удалять ед. хр.: \"шт\"");
                        db.Entry(unit).State = EntityState.Deleted;
                        db.SaveChanges();
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

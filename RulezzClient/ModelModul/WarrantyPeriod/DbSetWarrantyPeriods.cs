using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriods : IDbSetModel<WarrantyPeriods>
    {
        public ObservableCollection<WarrantyPeriods> Load()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<WarrantyPeriods>(db.WarrantyPeriods);
            }
        }

        public void Add(WarrantyPeriods obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.WarrantyPeriods.Add(obj);
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

        public void Update(WarrantyPeriods obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
                        var unit = db.WarrantyPeriods.Find(objId);
                        if (unit == null) throw new Exception("Удалить не получилось");
                        if (unit.Period == "Нет") throw new Exception("Нельзя удалять гарантийный период: \"Нет\"");
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

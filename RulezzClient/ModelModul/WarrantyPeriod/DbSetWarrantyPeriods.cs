using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriods : IDbSetModel<WarrantyPeriods>
    {
        public ObservableCollection<WarrantyPeriods> Load()
        {
            return new ObservableCollection<WarrantyPeriods>(AutomationAccountingGoodsEntities.GetInstance()
                .WarrantyPeriods);
        }

        public void Add(WarrantyPeriods obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    AutomationAccountingGoodsEntities.GetInstance().WarrantyPeriods.Add(obj);
                    AutomationAccountingGoodsEntities.GetInstance().SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Update(WarrantyPeriods obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var unit = AutomationAccountingGoodsEntities.GetInstance().WarrantyPeriods.Find(obj.Id);
                    if (unit == null) throw new Exception("Изменить не получилось");
                    if (unit.Period == "Нет") throw new Exception("Нельзя изменять гарантийный период: \"Нет\"");
                    unit.Period = obj.Period;
                    AutomationAccountingGoodsEntities.GetInstance().Entry(unit).State = EntityState.Modified;
                    AutomationAccountingGoodsEntities.GetInstance().SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Delete(int objId)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var unit = AutomationAccountingGoodsEntities.GetInstance().WarrantyPeriods.Find(objId);
                    if (unit == null) throw new Exception("Удалить не получилось");
                    if (unit.Period == "Нет") throw new Exception("Нельзя удалять гарантийный период: \"Нет\"");
                    AutomationAccountingGoodsEntities.GetInstance().Entry(unit).State = EntityState.Deleted;
                    AutomationAccountingGoodsEntities.GetInstance().SaveChanges();
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

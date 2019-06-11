using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStorages : IDbSetModel<UnitStorages>
    {
        public ObservableCollection<UnitStorages> Load()
        {
            return new ObservableCollection<UnitStorages>(AutomationAccountingGoodsEntities.GetInstance().UnitStorages);
        }

        public void Add(UnitStorages obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    AutomationAccountingGoodsEntities.GetInstance().UnitStorages.Add(obj);
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

        public void Update(UnitStorages obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var unit = AutomationAccountingGoodsEntities.GetInstance().UnitStorages.Find(obj.Id);
                    if (unit == null) throw new Exception("Изменить не получилось");
                    if (unit.Title == "шт") throw new Exception("Нельзя изменять ед. хр.: \"шт\"");
                    unit.Title = obj.Title;
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
                    var unit = AutomationAccountingGoodsEntities.GetInstance().UnitStorages.Find(objId);
                    if (unit == null) throw new Exception("Удалить не получилось");
                    if (unit.Title == "шт") throw new Exception("Нельзя удалять ед. хр.: \"шт\"");
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

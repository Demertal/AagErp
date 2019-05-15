using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriods : DbSetModel<WarrantyPeriods>
    {
        public async Task<int> Load()
        {
            List<WarrantyPeriods> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.WarrantyPeriods.Load();
                        return db.WarrantyPeriods.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<WarrantyPeriods> list = new ObservableCollection<WarrantyPeriods>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    list.Add(item);
                }
            }

            List = list;
            return List.Count;
        }

        public override void Add(WarrantyPeriods obj)
        {
            using (StoreEntities db = new StoreEntities())
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

        public override void Update(WarrantyPeriods obj)
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

        public override void Delete(int objId)
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

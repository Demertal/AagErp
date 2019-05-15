using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStorages : DbSetModel<UnitStorages>
    {
        public async Task<int> Load()
        {
            List<UnitStorages> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.UnitStorages.Load();
                        return db.UnitStorages.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<UnitStorages> list = new ObservableCollection<UnitStorages>();

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

        public override void Add(UnitStorages obj)
        {
            using (StoreEntities db = new StoreEntities())
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

        public override void Update(UnitStorages obj)
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

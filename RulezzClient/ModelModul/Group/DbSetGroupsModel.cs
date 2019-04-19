using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Group
{
    public class DbSetGroupsModel : DbSetModel<Groups>
    {
        public async Task<int> Load(Groups parentGroup = null)
        {
            bool Pre(Groups obj)
            {
                if (obj.IdParentGroup == null && parentGroup == null) return true;
                return obj.IdParentGroup != null && parentGroup != null &&
                       obj.IdParentGroup.Value == parentGroup.Id;
            }

            List<Groups> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.Groups.Include("Groups1").Where(Pre).ToList();
                }
            });
            ObservableCollection<Groups> list = new ObservableCollection<Groups>();

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

        public override void Add(Groups obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Groups.Add(obj);
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
                        var group = db.Groups.Find(objId);
                        db.Entry(group).State = EntityState.Deleted;
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

        public override void Update(Groups obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var modifi = db.Groups.Find(obj.Id);
                        if (modifi != null)
                        {
                            modifi.Title = obj.Title;
                            db.Entry(modifi).State = EntityState.Modified;
                        }
                        else throw new Exception("Изменение не удалось");
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

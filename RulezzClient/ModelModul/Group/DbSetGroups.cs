using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.Group
{
    public class DbSetGroups : IDbSetModel<Groups>
    {
        public ObservableCollection<Groups> Load(Groups parentGroup = null)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                bool Pre(Groups obj)
                {
                    return (obj.IdParentGroup == null && parentGroup == null) ||
                           (obj.IdParentGroup != null && parentGroup != null &&
                            obj.IdParentGroup.Value == parentGroup.Id);
                }

                return new ObservableCollection<Groups>(db.Groups.Include("Groups1").Where(Pre));
            }
        }

        public void Add(Groups obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.IdParentGroup == 0)
                        {
                            obj.IdParentGroup = null;
                        }

                        var group = db.Groups.Find(obj.IdParentGroup);
                        obj.Groups2 = group;
                        db.Entry(obj).State = EntityState.Added;
                        if(obj.Groups2 != null)
                            db.Entry(obj.Groups2).State = EntityState.Unchanged;
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

        public void Update(Groups obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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

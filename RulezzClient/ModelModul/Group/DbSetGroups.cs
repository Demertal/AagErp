using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ModelModul.Group
{
    public class DbSetGroups : DbSetModel<Groups>
    {
        public async Task LoadAsync(Groups parentGroup = null)
        {
            bool Pre(Groups obj)
            {
                return (obj.IdParentGroup == null && parentGroup == null) ||
                       (obj.IdParentGroup != null && parentGroup != null && obj.IdParentGroup.Value == parentGroup.Id);
            }
            using (StoreEntities db = new StoreEntities())
            {
                await db.Groups.Include("Groups1").LoadAsync();
                List = new ObservableCollection<Groups>(db.Groups.Local.Where(Pre));
            }
        }

        public override async Task AddAsync(Groups obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Groups.Add(obj);
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
                        var group = db.Groups.Find(objId);
                        db.Entry(group).State = EntityState.Deleted;
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

        public override async Task UpdateAsync(Groups obj)
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

using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Group
{
    public class DbSetGroups : AutomationAccountingGoodsEntities, IDbSetModel<Groups>
    {
        public async Task<ObservableCollection<Groups>> LoadAsync(Groups parentGroup = null)
        {
            bool Pre(Groups obj)
            {
                return (obj.IdParentGroup == null && parentGroup == null) ||
                       (obj.IdParentGroup != null && parentGroup != null && obj.IdParentGroup.Value == parentGroup.Id);
            }
            await Groups.Include("Groups1").LoadAsync();
            return new ObservableCollection<Groups>(Groups.Local.Where(Pre));
        }

        public async Task AddAsync(Groups obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    if (obj.IdParentGroup == 0)
                    {
                        obj.IdParentGroup = null;
                    }
                    var group = Groups.Find(obj.IdParentGroup);
                    obj.Groups2 = group;
                    Entry(obj).State = EntityState.Added;
                    Entry(obj.Groups2).State = EntityState.Unchanged;
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(int objId)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var group = Groups.Find(objId);
                    Entry(group).State = EntityState.Deleted;
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(Groups obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var modifi = Groups.Find(obj.Id);
                    if (modifi != null)
                    {
                        modifi.Title = obj.Title;
                        Entry(modifi).State = EntityState.Modified;
                    }
                    else throw new Exception("Изменение не удалось");
                    await SaveChangesAsync();
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

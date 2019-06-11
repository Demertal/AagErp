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
            bool Pre(Groups obj)
            {
                return (obj.IdParentGroup == null && parentGroup == null) ||
                       (obj.IdParentGroup != null && parentGroup != null && obj.IdParentGroup.Value == parentGroup.Id);
            }

            return new ObservableCollection<Groups>(AutomationAccountingGoodsEntities.GetInstance().Groups
                .Include("Groups1").Where(Pre));
        }

        public void Add(Groups obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    if (obj.IdParentGroup == 0)
                    {
                        obj.IdParentGroup = null;
                    }
                    var group = AutomationAccountingGoodsEntities.GetInstance().Groups.Find(obj.IdParentGroup);
                    obj.Groups2 = group;
                    AutomationAccountingGoodsEntities.GetInstance().Entry(obj).State = EntityState.Added;
                    AutomationAccountingGoodsEntities.GetInstance().Entry(obj.Groups2).State = EntityState.Unchanged;
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
                    var group = AutomationAccountingGoodsEntities.GetInstance().Groups.Find(objId);
                    AutomationAccountingGoodsEntities.GetInstance().Entry(group).State = EntityState.Deleted;
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

        public void Update(Groups obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var modifi = AutomationAccountingGoodsEntities.GetInstance().Groups.Find(obj.Id);
                    if (modifi != null)
                    {
                        modifi.Title = obj.Title;
                        AutomationAccountingGoodsEntities.GetInstance().Entry(modifi).State = EntityState.Modified;
                    }
                    else throw new Exception("Изменение не удалось");
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

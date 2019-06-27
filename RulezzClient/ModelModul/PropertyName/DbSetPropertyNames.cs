using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.PropertyName
{
    public class DbSetPropertyNames : IDbSetModel<PropertyNames>
    {
        public ObservableCollection<PropertyNames> Load(int idGroup)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<PropertyNames>(db.PropertyNames.Where(obj => obj.IdGroup == idGroup));
            }
        }

        public void Add(PropertyNames obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.Groups != null)
                        {
                            obj.IdGroup = obj.Groups.Id;
                            obj.Groups = null;
                        }

                        obj.PropertyValues = null;

                        List<int> groups = new List<int>();
                        groups.AddRange(GetChildGroup(obj.IdGroup));
                        groups.AddRange(GetParentGroup(obj.IdGroup));

                        if (db.PropertyNames.Any(objPr => objPr.Title == obj.Title && groups.Contains(objPr.IdGroup)))
                            throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                        db.PropertyNames.Add(obj);
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

        public ObservableCollection<PropertyNames> GetAllPropertyNames(int idGroup)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                List<int> groups = GetParentGroup(idGroup);
                return new ObservableCollection<PropertyNames>(db.PropertyNames
                    .Where(objPr => groups.Contains(objPr.IdGroup)).Include(objPr => objPr.PropertyValues));
            }
        }

        private IEnumerable<int> GetChildGroup(int idParentGroup)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                List<int> groups = db.Groups.Where(objGr => objGr.IdParentGroup == idParentGroup)
                    .Select(objGr => objGr.Id).ToList();
                List<int> temp = new List<int>();
                foreach (var t in groups)
                {
                    temp.AddRange(GetChildGroup(t));
                }

                groups.AddRange(temp);
                return groups;
            }
        }

        private List<int> GetParentGroup(int? id)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                List<int> groups = new List<int>();
                do
                {
                    Groups temp = db.Groups.SingleOrDefault(objGr => objGr.Id == id);
                    if (temp == null) break;
                    groups.Add(temp.Id);
                    id = temp.IdParentGroup;
                } while (id != null);

                groups.AddRange(groups);
                return groups;
            }
        }

        public void Update(PropertyNames obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var property = db.PropertyNames.Find(obj.Id);
                        if (property == null) throw new Exception("Изменить не получилось");

                        List<int> groups = new List<int>();
                        groups.AddRange(GetChildGroup(obj.IdGroup));
                        groups.AddRange(GetParentGroup(obj.IdGroup));

                        if (db.PropertyNames.Any(objPr => objPr.Title == obj.Title && groups.Contains(objPr.IdGroup)))
                            throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                        property.Title = obj.Title;
                        db.Entry(property).State = EntityState.Modified;
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
                        var propertyNames = db.PropertyNames.Find(objId);
                        db.Entry(propertyNames).State = EntityState.Deleted;
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

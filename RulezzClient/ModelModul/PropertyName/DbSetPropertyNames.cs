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
            return new ObservableCollection<PropertyNames>(AutomationAccountingGoodsEntities.GetInstance().PropertyNames
                .Where(obj => obj.IdGroup == idGroup));
        }

        public void Add(PropertyNames obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
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

                    if (AutomationAccountingGoodsEntities.GetInstance().PropertyNames.Any(objPr => objPr.Title == obj.Title &&  groups.Contains(objPr.IdGroup)))
                        throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                    AutomationAccountingGoodsEntities.GetInstance().PropertyNames.Add(obj);
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

        public ObservableCollection<PropertyNames> GetAllPropertyNames(int idGroup)
        {
            List<int> groups = GetParentGroup(idGroup);
            return new ObservableCollection<PropertyNames>(AutomationAccountingGoodsEntities.GetInstance().PropertyNames
                .Where(objPr => groups.Contains(objPr.IdGroup)).Include(objPr => objPr.PropertyValues));
        }

        private List<int> GetChildGroup(int idParentGroup)
        {
            List<int> groups = AutomationAccountingGoodsEntities.GetInstance().Groups.Where(objGr => objGr.IdParentGroup == idParentGroup).Select(objGr => objGr.Id)
                .ToList();
            List<int> temp = new List<int>();
            foreach (var t in groups)
            {
                temp.AddRange(GetChildGroup(t));
            }
            groups.AddRange(temp);
            return groups;
        }

        private List<int> GetParentGroup(int? id)
        {
            List<int> groups = new List<int>();
            do
            {
                Groups temp = AutomationAccountingGoodsEntities.GetInstance().Groups.SingleOrDefault(objGr => objGr.Id == id);
                if(temp == null) break;
                groups.Add(temp.Id);
                id = temp.IdParentGroup;
            } while (id != null);
            groups.AddRange(groups);
            return groups;
        }

        public void Update(PropertyNames obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var property = AutomationAccountingGoodsEntities.GetInstance().PropertyNames.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                    List<int> groups = new List<int>();
                    groups.AddRange(GetChildGroup(obj.IdGroup));
                    groups.AddRange(GetParentGroup(obj.IdGroup));

                    if (AutomationAccountingGoodsEntities.GetInstance().PropertyNames.Any(objPr => objPr.Title == obj.Title && groups.Contains(objPr.IdGroup)))
                        throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                    property.Title = obj.Title;
                    AutomationAccountingGoodsEntities.GetInstance().Entry(property).State = EntityState.Modified;
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
                    var propertyNames = AutomationAccountingGoodsEntities.GetInstance().PropertyNames.Find(objId);
                    AutomationAccountingGoodsEntities.GetInstance().Entry(propertyNames).State = EntityState.Deleted;
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

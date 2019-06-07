using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.PropertyName
{
    public class DbSetPropertyNames : AutomationAccountingGoodsEntities, IDbSetModel<PropertyNames>
    {
        public async Task<ObservableCollection<PropertyNames>> LoadAsync(int idGroup)
        {
            await PropertyNames.Where(obj => obj.IdGroup == idGroup).LoadAsync();
            return PropertyNames.Local;
        }

        public async Task AddAsync(PropertyNames obj)
        {
            using (var transaction = Database.BeginTransaction())
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

                    if (await PropertyNames.AnyAsync(objPr => objPr.Title == obj.Title &&  groups.Contains(objPr.IdGroup)))
                        throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                    PropertyNames.Add(obj);
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

        public async Task<ObservableCollection<PropertyNames>> GetAllPropertyNames(int idGroup)
        {
            List<int> groups = GetParentGroup(idGroup);
            await PropertyNames.Where(objPr => groups.Contains(objPr.IdGroup)).Include(objPr => objPr.PropertyValues).LoadAsync();
            return PropertyNames.Local;
        }

        private List<int> GetChildGroup(int idParentGroup)
        {
            List<int> groups = Groups.Where(objGr => objGr.IdParentGroup == idParentGroup).Select(objGr => objGr.Id)
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
                Groups temp = Groups.FirstOrDefault(objGr => objGr.Id == id);
                if(temp == null) break;
                groups.Add(temp.Id);
                id = temp.IdParentGroup;
            } while (id != null);
            groups.AddRange(groups);
            return groups;
        }

        public async Task UpdateAsync(PropertyNames obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var property = PropertyNames.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                    List<int> groups = new List<int>();
                    groups.AddRange(GetChildGroup(obj.IdGroup));
                    groups.AddRange(GetParentGroup(obj.IdGroup));

                    if (await PropertyNames.AnyAsync(objPr => objPr.Title == obj.Title && groups.Contains(objPr.IdGroup)))
                        throw new Exception("Такой параметр уже есть в этой иеррахии групп");

                    property.Title = obj.Title;
                    Entry(property).State = EntityState.Modified;
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
                    var propertyNames = PropertyNames.Find(objId);
                    Entry(propertyNames).State = EntityState.Deleted;
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

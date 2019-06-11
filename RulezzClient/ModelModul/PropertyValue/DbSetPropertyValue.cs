using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.PropertyValue
{
    public class DbSetPropertyValue : IDbSetModel<PropertyValues>
    {
        public ObservableCollection<PropertyValues> Load(int idPropertyName)
        {
            return new ObservableCollection<PropertyValues>(AutomationAccountingGoodsEntities.GetInstance()
                .PropertyValues.Where(obj => obj.IdPropertyName == idPropertyName));
        }

        public void Add(PropertyValues obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    if (obj.PropertyNames != null)
                    {
                        obj.IdPropertyName = obj.PropertyNames.Id;
                        obj.PropertyNames = null;
                    }

                    AutomationAccountingGoodsEntities.GetInstance().PropertyValues.Add(obj);
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

        public void Update(PropertyValues obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var property = AutomationAccountingGoodsEntities.GetInstance().PropertyValues.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                       property.Value = obj.Value;
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
                    var property = AutomationAccountingGoodsEntities.GetInstance().PropertyValues.Find(objId);
                    AutomationAccountingGoodsEntities.GetInstance().Entry(property).State = EntityState.Deleted;
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

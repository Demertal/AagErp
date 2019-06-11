using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.PropertyProduct
{
    public class DbSetPropertyProducts : IDbSetModel<PropertyProducts>
    {
        public ObservableCollection<PropertyProducts> Load(int idProduct)
        {
            return new ObservableCollection<PropertyProducts>(AutomationAccountingGoodsEntities.GetInstance()
                .PropertyProducts.Where(obj => obj.IdProduct == idProduct).Include(obj => obj.PropertyNames)
                .Include(obj => obj.PropertyNames.PropertyValues));
        }

        public void Add(PropertyProducts obj)
        {
            throw new NotImplementedException();
        }

        public void Update(PropertyProducts obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var property = AutomationAccountingGoodsEntities.GetInstance().PropertyProducts.Find(obj.Id);
                    if (property == null) throw new Exception("Изменить не получилось");

                    property.IdPropertyValue = obj.IdPropertyValue;
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

        public void Update(List<PropertyProducts> objList)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in objList)
                    {
                        var property = AutomationAccountingGoodsEntities.GetInstance().PropertyProducts.Find(obj.Id);
                        if (property == null) throw new Exception("Изменить не получилось");

                        property.IdPropertyValue = obj.IdPropertyValue;
                        AutomationAccountingGoodsEntities.GetInstance().Entry(property).State = EntityState.Modified;
                    }
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
            throw new NotImplementedException();
        }
    }
}

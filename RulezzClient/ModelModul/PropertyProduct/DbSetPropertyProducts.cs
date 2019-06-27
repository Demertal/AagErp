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
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<PropertyProducts>(db.PropertyProducts
                    .Where(obj => obj.IdProduct == idProduct).Include(obj => obj.PropertyNames)
                    .Include(obj => obj.PropertyNames.PropertyValues));
            }
        }

        public void Add(PropertyProducts obj)
        {
            throw new NotImplementedException();
        }

        public void Update(PropertyProducts obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var property = db.PropertyProducts.Find(obj.Id);
                        if (property == null) throw new Exception("Изменить не получилось");

                        property.IdPropertyValue = obj.IdPropertyValue;
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

        public void Update(List<PropertyProducts> objList)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var obj in objList)
                        {
                            var property = db.PropertyProducts
                                .Find(obj.Id);
                            if (property == null) throw new Exception("Изменить не получилось");

                            property.IdPropertyValue = obj.IdPropertyValue;
                            db.Entry(property).State = EntityState.Modified;
                        }

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
            throw new NotImplementedException();
        }
    }
}

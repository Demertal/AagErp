using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.Warranty
{
    public class DbSetWarranties : IDbSetModel<Warranties>
    {
        public ObservableCollection<Warranties> Load(int idSerials)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Warranties>(db.Warranties.Where(obj =>
                    obj.IdSerialNumber == idSerials));
            }
        }

        public Warranties Find(int id)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Warranties.SingleOrDefault(obj => obj.Id == id);
            }
        }

        public void Add(Warranties obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Warranties.Add(obj);
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

        public void Update(Warranties obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var warranty = db.Warranties.Find(obj.Id);
                        if (warranty == null) throw new Exception("Изменить не получилось");
                        warranty.Malfunction = obj.Malfunction;
                        warranty.Info = obj.Info;
                        warranty.DateDeparture = obj.DateDeparture;
                        warranty.DateIssue = obj.DateIssue;
                        warranty.DateReceipt = obj.DateReceipt;
                        db.Entry(warranty).State = EntityState.Modified;
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

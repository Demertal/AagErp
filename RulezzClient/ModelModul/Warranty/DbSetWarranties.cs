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
            return new ObservableCollection<Warranties>(AutomationAccountingGoodsEntities.GetInstance().Warranties
                .Where(obj => obj.IdSerialNumber == idSerials));
        }

        public Warranties Find(int id)
        {
            return AutomationAccountingGoodsEntities.GetInstance().Warranties.SingleOrDefault(obj => obj.Id == id);
        }

        public void Add(Warranties obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    AutomationAccountingGoodsEntities.GetInstance().Warranties.Add(obj);
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

        public void Update(Warranties obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var warranty = AutomationAccountingGoodsEntities.GetInstance().Warranties.Find(obj.Id);
                    if (warranty == null) throw new Exception("Изменить не получилось");
                    warranty.Malfunction = obj.Malfunction;
                    warranty.Info = obj.Info;
                    AutomationAccountingGoodsEntities.GetInstance().Entry(warranty).State = EntityState.Modified;
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

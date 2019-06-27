using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.ExchangeRate
{
    public class DbSetExchangeRates: IDbSetModel<ExchangeRates>
    {
        public ObservableCollection<ExchangeRates> Load()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<ExchangeRates>(db.ExchangeRates.ToList());
            }
        }

        public ExchangeRates Load(string title)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.ExchangeRates.FirstOrDefault(ex => ex.Title == title);
            }
        }

        public void Add(ExchangeRates obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.ExchangeRates.Add(obj);
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

        public void Update(ExchangeRates obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var modifi = db.ExchangeRates.Find(obj.Id);
                        if (modifi != null)
                        {
                            modifi.Title = obj.Title;
                            modifi.Course = obj.Course;
                            db.Entry(modifi).State = EntityState.Modified;
                        }
                        else throw new Exception("Изменение не удалось");

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

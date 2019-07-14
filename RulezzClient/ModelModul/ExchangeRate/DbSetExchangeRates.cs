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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<ExchangeRates>(db.ExchangeRates.ToList());
            }
        }

        public ExchangeRates Load(string title)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.ExchangeRates.FirstOrDefault(ex => ex.Title == title);
            }
        }

        public void Add(ExchangeRates obj)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var modifi = db.ExchangeRates.Find(obj.Id);
                        if (modifi == null) throw new Exception("Изменение не удалось");
                        modifi.Title = obj.Title;
                        modifi.Course = obj.Course;
                        modifi.IsDefault = obj.IsDefault;
                        db.Entry(modifi).State = EntityState.Modified;
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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var exchangeRate = db.ExchangeRates.Find(objId);
                        if (exchangeRate == null) throw new Exception("Валюта не найдена");
                        db.Entry(exchangeRate).State = EntityState.Deleted;
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

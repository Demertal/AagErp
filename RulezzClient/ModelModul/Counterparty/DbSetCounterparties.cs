using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.Counterparty
{
    public class DbSetCounterparties: IDbSetModel<Counterparties>
    {
        public ObservableCollection<Counterparties> Load()
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Counterparties>(db.Counterparties.ToList());
            }
        }

        public ObservableCollection<Counterparties> Load(TypeCounterparties whoIsIt)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Counterparties>(db.Counterparties.Where(obj => obj.WhoIsIt == (int)whoIsIt)
                    .ToList());
            }
        }

        public void Add(Counterparties obj)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Counterparties.Add(obj);
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

        public void Update(Counterparties obj)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var counterparty = db.Counterparties.Find(obj.Id);
                        if (counterparty == null) throw new Exception("Изменение не удалось");
                        counterparty.Title = obj.Title;
                        counterparty.Address = obj.Address;
                        counterparty.ContactPerson = obj.ContactPerson;
                        counterparty.ContactPhone = obj.ContactPhone;
                        counterparty.Props = obj.Props;
                        counterparty.Debt = obj.Debt;
                        db.Entry(counterparty).State = EntityState.Modified;
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
                        var counterparties = db.Counterparties.Find(objId);
                        if (counterparties == null) throw new Exception("Контрагент не найден");
                        db.Entry(counterparties).State = EntityState.Deleted;
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

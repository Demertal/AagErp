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
            using (AutomationAccountingGoodsEntities db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Counterparties>(db.Counterparties.ToList());
            }
        }

        public ObservableCollection<Counterparties> Load(TypeCounterparties whoIsIt)
        {
            bool temp = whoIsIt == TypeCounterparties.Buyers;
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Counterparties>(db.Counterparties.Where(obj => obj.WhoIsIt == temp)
                    .ToList());
            }
        }

        public void Add(Counterparties obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var counterparty = db.Counterparties.Find(obj.Id);
                        if (counterparty != null)
                        {
                            counterparty.Title = obj.Title;
                            counterparty.Address = obj.Address;
                            counterparty.ContactPerson = obj.ContactPerson;
                            counterparty.ContactPhone = obj.ContactPhone;
                            counterparty.Props = obj.Props;
                            db.Entry(counterparty).State = EntityState.Modified;
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
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var counterparties = db.Counterparties.Find(objId);
                        if (counterparties == null) throw new Exception("Контрагент не найден");
                        if (counterparties.Title == "Покупатель")
                            throw new Exception("Нельзя удалить контрагента \"Покупатель\"");
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

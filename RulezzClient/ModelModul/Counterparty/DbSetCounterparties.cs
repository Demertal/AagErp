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
            return new ObservableCollection<Counterparties>(AutomationAccountingGoodsEntities.GetInstance()
                .Counterparties.ToList());
        }

        public ObservableCollection<Counterparties> Load(TypeCounterparties whoIsIt)
        {
            bool temp = whoIsIt == TypeCounterparties.Buyers;
            return new ObservableCollection<Counterparties>(AutomationAccountingGoodsEntities.GetInstance()
                .Counterparties.Where(obj => obj.WhoIsIt == temp).ToList());
        }

        public void Add(Counterparties obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    AutomationAccountingGoodsEntities.GetInstance().Counterparties.Add(obj);
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

        public void Update(Counterparties obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var counterparty = AutomationAccountingGoodsEntities.GetInstance().Counterparties.Find(obj.Id);
                    if (counterparty != null)
                    {
                        counterparty.Title = obj.Title;
                        counterparty.Address = obj.Address;
                        counterparty.ContactPerson = obj.ContactPerson;
                        counterparty.ContactPhone = obj.ContactPhone;
                        counterparty.Props = obj.Props;
                        AutomationAccountingGoodsEntities.GetInstance().Entry(counterparty).State = EntityState.Modified;
                    }
                    else throw new Exception("Изменение не удалось");
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
                    var counterparties = AutomationAccountingGoodsEntities.GetInstance().Counterparties.Find(objId);
                    if(counterparties == null) throw new Exception("Контрагент не найден");
                    if (counterparties.Title == "Покупатель") throw new Exception("Нельзя удалить контрагента \"Покупатель\"");
                    AutomationAccountingGoodsEntities.GetInstance().Entry(counterparties).State = EntityState.Deleted;
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

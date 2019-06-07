using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Counterparty
{
    public class DbSetCounterparties: AutomationAccountingGoodsEntities, IDbSetModel<Counterparties>
    {
        public async Task<ObservableCollection<Counterparties>> LoadAsync()
        {
            await Counterparties.LoadAsync();
            return Counterparties.Local;
        }

        public async Task<ObservableCollection<Counterparties>> LoadAsync(TypeCounterparties whoIsIt)
        {
            bool temp = whoIsIt == TypeCounterparties.Buyers;
            await Counterparties.Where(obj => obj.WhoIsIt == temp).LoadAsync();
            return Counterparties.Local;
        }

        public async Task AddAsync(Counterparties obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Counterparties.Add(obj);
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(Counterparties obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var counterparty = Counterparties.Find(obj.Id);
                    if (counterparty != null)
                    {
                        counterparty.Title = obj.Title;
                        counterparty.Address = obj.Address;
                        counterparty.ContactPerson = obj.ContactPerson;
                        counterparty.ContactPhone = obj.ContactPhone;
                        counterparty.Props = obj.Props;
                        Entry(counterparty).State = EntityState.Modified;
                    }
                    else throw new Exception("Изменение не удалось");
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(int objId)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var counterparties = Counterparties.Find(objId);
                    if(counterparties == null) throw new Exception("Контрагент не найден");
                    if (counterparties.Title == "Покупатель") throw new Exception("Нельзя удалить контрагента \"Покупатель\"");
                    Entry(counterparties).State = EntityState.Deleted;
                    await SaveChangesAsync();
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

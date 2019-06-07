using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Warranty
{
    public class DbSetWarranties : AutomationAccountingGoodsEntities, IDbSetModel<Warranties>
    {
        public async Task<ObservableCollection<Warranties>> LoadAsync(int idSerials)
        {
            await Warranties.Where(obj => obj.IdSerialNumber == idSerials).LoadAsync();
            return Warranties.Local;
        }

        public async Task<Warranties> FindAsync(int id)
        {
            return await Warranties.Where(obj => obj.Id == id).SingleAsync();
        }

        public async Task AddAsync(Warranties obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Warranties.Add(obj);
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

        public async Task UpdateAsync(Warranties obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var warranty = Warranties.Find(obj.Id);
                    if (warranty == null) throw new Exception("Изменить не получилось");
                    warranty.Malfunction = obj.Malfunction;
                    warranty.Info = obj.Info;
                    Entry(warranty).State = EntityState.Modified;
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

        public Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

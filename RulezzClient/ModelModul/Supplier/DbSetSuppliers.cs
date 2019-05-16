using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.Supplier
{
    public class DbSetSuppliers: DbSetModel<Suppliers>
    {
        public async Task LoadAsync()
        {
            using (StoreEntities db = new StoreEntities())
            {
                await db.Suppliers.LoadAsync();
                List = db.Suppliers.Local;
            }
        }

        public override async Task AddAsync(Suppliers obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Suppliers.Add(obj);
                        await db.SaveChangesAsync();
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

        public override async Task UpdateAsync(Suppliers obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var supplier = db.Suppliers.Find(obj.Id);
                        if (supplier != null)
                        {
                            supplier.Title = obj.Title;
                            db.Entry(supplier).State = EntityState.Modified;
                        }
                        else throw new Exception("Изменение не удалось");
                        await db.SaveChangesAsync();
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

        public override async Task DeleteAsync(int objId)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var supplier = db.Suppliers.Find(objId);
                        db.Entry(supplier).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
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

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Product
{
    public class DbSetProducts : DbSetModel<Products>
    {
        public async Task LoadAsync(int idGroup, string findString)
        {
            using (StoreEntities db = new StoreEntities())
            {

                if (string.IsNullOrEmpty(findString))
                {
                    await db.Products.Where(obj => obj.IdGroup == idGroup).Include(p => p.CountProducts)
                        .Include(p => p.ExchangeRates).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                        .Include(p => p.Groups).LoadAsync();
                }
                else
                {
                    await db.Products.Include(p => p.CountProducts)
                        .Where(obj =>
                            obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                            obj.Barcode.Contains(findString)).Include(p => p.ExchangeRates)
                        .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups)
                        .LoadAsync();
                }

                List = db.Products.Local;
            }
        }

        public override async Task DeleteAsync(int id)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var product = db.Products.Find(id);
                        db.Entry(product).State = EntityState.Deleted;
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

        public override async Task AddAsync(Products product)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (product.Groups != null)
                        {
                            product.IdGroup = product.Groups.Id;
                            product.Groups = null;
                        }
                        if (product.WarrantyPeriods != null)
                        {
                            product.IdWarrantyPeriod = product.WarrantyPeriods.Id;
                            product.WarrantyPeriods = null;
                        }
                        if (product.UnitStorages != null)
                        {
                            product.IdUnitStorage = product.UnitStorages.Id;
                            product.UnitStorages = null;
                        }
                        product.IdExchangeRate = db.ExchangeRates.Select(ex => ex.Id).FirstOrDefault();

                        db.Products.Add(product);
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

        public override async Task UpdateAsync(Products product)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (product.Groups != null)
                        {
                            product.IdGroup = product.Groups.Id;
                            product.Groups = null;
                        }
                        if (product.WarrantyPeriods != null)
                        {
                            product.IdWarrantyPeriod = product.WarrantyPeriods.Id;
                            product.WarrantyPeriods = null;
                        }
                        if (product.UnitStorages != null)
                        {
                            product.IdUnitStorage = product.UnitStorages.Id;
                            product.UnitStorages = null;
                        }
                        db.Entry(product).State = EntityState.Modified;
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

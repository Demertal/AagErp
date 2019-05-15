using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Product
{
    public class DbSetProducts : DbSetModel<Products>
    {

        #region LoadMethod

        public async Task<int> Load(int idGroup, string findString)
        {
            List<Products> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        if (string.IsNullOrEmpty(findString))
                        {
                            return db.Products.Include(p => p.CountProducts).Include(p => p.ExchangeRates)
                                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups).Where(obj => obj.IdGroup == idGroup).ToList();
                        }
                        else
                        {
                            return db.Products.Include(p => p.CountProducts).Include(p => p.ExchangeRates)
                                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups).Where(obj =>
                                    obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                                    obj.Barcode.Contains(findString)).ToList();
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<Products> list = new ObservableCollection<Products>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    list.Add(item);
                }
            }

            List = list;
            return List.Count;
        }

        #endregion

        public override void Delete(int id)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var product = db.Products.Find(id);
                        db.Entry(product).State = EntityState.Deleted;
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

        public override void Add(Products product)
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

        public override void Update(Products product)
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

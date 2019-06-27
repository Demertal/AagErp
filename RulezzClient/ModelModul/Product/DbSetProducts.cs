using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace ModelModul.Product
{
    public class DbSetProducts : IDbSetModel<Products>
    {
        public ObservableCollection<Products> Load(int idGroup, string findString)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                if (string.IsNullOrEmpty(findString))
                {
                    return new ObservableCollection<Products>(db.Products.Where(obj => obj.IdGroup == idGroup)
                        .Include(p => p.CountProducts).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                        .Include(p => p.Groups));
                }

                return new ObservableCollection<Products>(db.Products.Include(p => p.CountProducts)
                    .Where(obj =>
                        obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                        obj.Barcode.Contains(findString)).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                    .Include(p => p.Groups));
            }
        }

        public bool CheckBarcode(string barcode)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Any(obj => obj.Barcode == barcode);
            }
        }

        public Products FindProductByBarcode(string barcode)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Include(p => p.CountProducts).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups).SingleOrDefault(obj => obj.Barcode == barcode);
            }
        }

        public Products FindProductById(int id)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Include(p => p.CountProducts).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups).Single(obj => obj.Id == id);
            }
        }

        public PurchaseStruct GetPurchasePrice(int id, int last = 0)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                PurchaseInfos temp = db.PurchaseInfos.Include(obj => obj.PurchaseReports)
                    .Include(obj => obj.ExchangeRates)
                    .OrderByDescending(obj => obj.PurchaseReports.DataOrder.Value.Year)
                    .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Month)
                    .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Day)
                    .ThenByDescending(obj => obj.PurchaseReports.Id).Skip(last)
                    .FirstOrDefault(obj => obj.IdProduct == id);
                return temp == null
                    ? null
                    : new PurchaseStruct {ExchangeRate = temp.ExchangeRates, PurchasePrice = temp.PurchasePrice};
            }
        }

        public ObservableCollection<string> GetCountProduct(int id)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                ObservableCollection<string> temp = new ObservableCollection<string>();
                db.CountProducts.Where(objCt => objCt.IdProduct == id).Include(objCt => objCt.Stores).ToList().ForEach(
                    obj => temp.Add(obj.Stores.Title + ": " + obj.Count.ToString(CultureInfo.InvariantCulture)));
                return temp;
            }
        }

        public void Delete(int id)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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

        public void Add(Products product)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
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

        public void Update(Products product)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Products temp = db.Products.Find(product.Id);
                        if (temp == null) throw new Exception("Изменение не удалось");
                        temp.IdGroup = product.Groups?.Id ?? product.IdGroup;
                        temp.IdWarrantyPeriod = product.WarrantyPeriods?.Id ?? product.IdWarrantyPeriod;
                        temp.IdUnitStorage = product.UnitStorages?.Id ?? product.IdUnitStorage;
                        temp.Groups = null;
                        temp.CountProducts = product.CountProducts;
                        temp.WarrantyPeriods = null;
                        temp.UnitStorages = null;
                        temp.Barcode = product.Barcode;
                        temp.Title = product.Title;
                        temp.VendorCode = product.VendorCode;
                        temp.PropertyProducts = null;
                        temp.PurchaseInfos = null;
                        temp.RevaluationProductsInfos = null;
                        db.Entry(temp).State = EntityState.Modified;
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

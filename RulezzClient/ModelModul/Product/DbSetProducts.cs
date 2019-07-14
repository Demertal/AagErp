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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                if (string.IsNullOrEmpty(findString))
                {
                    return new ObservableCollection<Products>(db.Products.Where(obj => obj.IdGroup == idGroup)
                        .Include(p => p.CountProducts).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                        .Include(p => p.Groups).Include(p => p.PriceGroups));
                }

                return new ObservableCollection<Products>(db.Products.Include(p => p.CountProducts)
                    .Where(obj =>
                        obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                        obj.Barcode.Contains(findString)).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                    .Include(p => p.Groups).Include(p => p.PriceGroups));
            }
        }

        public bool CheckBarcode(string barcode)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Any(obj => obj.Barcode == barcode);
            }
        }

        public Products FindProductByBarcode(string barcode)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Include(p => p.CountProducts).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups).Include(p => p.PriceGroups).SingleOrDefault(obj => obj.Barcode == barcode);
            }
        }

        public Products FindProductById(int id)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.Products.Include(p => p.CountProducts).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups).Include(p => p.PriceGroups).Single(obj => obj.Id == id);
            }
        }

        public PurchaseStruct GetPurchasePrice(int id, int last = 0)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                MovementGoodsInfos temp = db.MovementGoodsInfos.Include(obj => obj.MovementGoodsReports).Where(obj => obj.MovementGoodsReports.TypeAction == 0)
                    .Include(obj => obj.MovementGoodsReports.ExchangeRates)
                    .OrderByDescending(obj => obj.MovementGoodsReports.Date)
                    .ThenByDescending(obj => obj.MovementGoodsReports.Id).Skip(last)
                    .FirstOrDefault(obj => obj.IdProduct == id);
                return temp == null
                    ? null
                    : new PurchaseStruct {ExchangeRate = temp.MovementGoodsReports.ExchangeRates, PurchasePrice = temp.Price.Value};
            }
        }

        public ObservableCollection<string> GetCountProduct(int id)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                ObservableCollection<string> temp = new ObservableCollection<string>();
                db.CountProducts.Where(objCt => objCt.IdProduct == id).Include(objCt => objCt.Stores).ToList().ForEach(
                    obj => temp.Add(obj.Stores.Title + ": " + obj.Count.ToString(CultureInfo.InvariantCulture)));
                return temp;
            }
        }

        public void Delete(int id)
        {
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
            using (var db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
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
                        temp.InvoiceInfos = null;
                        temp.PriceProducts = null;
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

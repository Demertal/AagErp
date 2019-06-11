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
            if (string.IsNullOrEmpty(findString))
            {
                return new ObservableCollection<Products>(AutomationAccountingGoodsEntities.GetInstance().Products
                    .Where(obj => obj.IdGroup == idGroup).Include(p => p.CountProducts).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups));
            }

            return new ObservableCollection<Products>(AutomationAccountingGoodsEntities.GetInstance().Products
                .Include(p => p.CountProducts)
                .Where(obj =>
                    obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                    obj.Barcode.Contains(findString)).Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                .Include(p => p.Groups));
        }

        public bool CheckBarcode(string barcode)
        {
            return AutomationAccountingGoodsEntities.GetInstance().Products.Any(obj => obj.Barcode == barcode);
        }

        public Products FindProductByBarcode(string barcode)
        {
            return AutomationAccountingGoodsEntities.GetInstance().Products.Include(p => p.CountProducts)
                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups)
                .SingleOrDefault(obj => obj.Barcode == barcode);
        }

        public Products FindProductById(int id)
        {
            return AutomationAccountingGoodsEntities.GetInstance().Products.Include(p => p.CountProducts)
                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups)
                .Single(obj => obj.Id == id);
        }

        public PurchaseStruct GetPurchasePrice(int id, int last = 0)
        {
            PurchaseInfos temp = AutomationAccountingGoodsEntities.GetInstance().PurchaseInfos.Include(obj => obj.PurchaseReports)
                .Include(obj => obj.ExchangeRates).OrderByDescending(obj => obj.PurchaseReports.DataOrder.Value.Year)
                .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Month)
                .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Day)
                .ThenByDescending(obj => obj.PurchaseReports.Id).Skip(last).FirstOrDefault(obj => obj.IdProduct == id);
            return temp == null ? null : new PurchaseStruct{ExchangeRate = temp.ExchangeRates, PurchasePrice = temp.PurchasePrice};
        }

        public ObservableCollection<string> GetCountProduct(int id)
        {
            ObservableCollection<string> temp = new ObservableCollection<string>();
            AutomationAccountingGoodsEntities.GetInstance().CountProducts.Where(objCt => objCt.IdProduct == id)
                .Include(objCt => objCt.Stores).ToList().ForEach(obj =>
                    temp.Add(obj.Stores.Title + ": " + obj.Count.ToString(CultureInfo.InvariantCulture)));
            return temp;
        }

        public void Delete(int id)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    var product = AutomationAccountingGoodsEntities.GetInstance().Products.Find(id);
                    AutomationAccountingGoodsEntities.GetInstance().Entry(product).State = EntityState.Deleted;
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

        public void Add(Products product)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    AutomationAccountingGoodsEntities.GetInstance().Products.Add(product);
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

        public void Update(Products product)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    Products temp = AutomationAccountingGoodsEntities.GetInstance().Products.Find(product.Id);
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
                    AutomationAccountingGoodsEntities.GetInstance().Entry(temp).State = EntityState.Modified;
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

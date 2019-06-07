using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Product
{
    public class DbSetProducts : AutomationAccountingGoodsEntities, IDbSetModel<Products>
    {
        public async Task<ObservableCollection<Products>> LoadAsync(int idGroup, string findString)
        {
            if (string.IsNullOrEmpty(findString))
            {
                await Products.Where(obj => obj.IdGroup == idGroup).Include(p => p.CountProducts)
                    .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages)
                    .Include(p => p.Groups).LoadAsync();
            }
            else
            {
                await Products.Include(p => p.CountProducts)
                    .Where(obj =>
                        obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                        obj.Barcode.Contains(findString)).Include(p => p.WarrantyPeriods)
                    .Include(p => p.UnitStorages).Include(p => p.Groups).LoadAsync();
            }

            return Products.Local;
        }

        public async Task<int> CheckBarcodeAsync(string barcode)
        {
            return await Products.Where(obj => obj.Barcode == barcode).CountAsync();
        }

        public async Task<Products> FindProductByBarcodeAsync(string barcode)
        {
            return await Products.Where(obj => obj.Barcode == barcode).Include(p => p.CountProducts)
                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups).SingleAsync();
        }

        public async Task<Products> FindProductByIdAsync(int id)
        {
            return await Products.Where(obj => obj.Id == id).Include(p => p.CountProducts)
                .Include(p => p.WarrantyPeriods).Include(p => p.UnitStorages).Include(p => p.Groups).SingleAsync();
        }

        public async Task<PurchaseStruct> GetPurchasePriceAsync(int id, int last = 0)
        {
            return await PurchaseInfos.Where(obj => obj.IdProduct == id).Include(obj => obj.PurchaseReports)
                .Include(obj => obj.ExchangeRates).OrderByDescending(obj => obj.PurchaseReports.DataOrder.Value.Year)
                .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Month)
                .ThenByDescending(obj => obj.PurchaseReports.DataOrder.Value.Day)
                .ThenByDescending(obj => obj.PurchaseReports.Id).Skip(last).Select(obj =>
                    new PurchaseStruct { PurchasePrice = obj.PurchasePrice, ExchangeRate = obj.ExchangeRates })
                .FirstOrDefaultAsync();
        }

        public async Task<ObservableCollection<string>> GetCountProduct(int id)
        {
            await CountProducts.Where(objCt => objCt.IdProduct == id).Include(objCt => objCt.Stores).LoadAsync();
            ObservableCollection<string> temp = new ObservableCollection<string>();
            foreach (var countProduct in CountProducts.Local)
            {
                temp.Add(countProduct.Stores.Title + ": " + countProduct.Count.ToString(CultureInfo.InvariantCulture));
            }
            return temp;
        }

        public async Task DeleteAsync(int id)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var product = Products.Find(id);
                    Entry(product).State = EntityState.Deleted;
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

        public async Task AddAsync(Products product)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Products.Add(product);
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

        public async Task UpdateAsync(Products product)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Products temp = Products.Find(product.Id);
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
                    Entry(temp).State = EntityState.Modified;
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

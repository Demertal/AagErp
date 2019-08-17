using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlProductRepository : SqlRepository<Product>
    {
        //public PurchaseStruct GetPurchasePrice(int id, int last = 0)
        //{
        //    using (var db = new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
        //    {
        //        MovementGoodsInfo temp = db.MovementGoodsInfos.Include(obj => obj.MovementGoods).Where(obj => obj.MovementGoods.TypeAction == 0)
        //            .Include(obj => obj.MovementGoods.Currencies)
        //            .OrderByDesc(obj => obj.MovementGoods.DateCreate)
        //            .ThenByDesc(obj => obj.MovementGoods.Id).Skip(last)
        //            .FirstOrDefault(obj => obj.IdProduct == id);
        //        return temp == null
        //            ? null
        //            : new PurchaseStruct {Currency = temp.MovementGoods.Currencies, PurchasePrice = temp.Price.Value};
        //    }
        //}

        public override async Task UpdateAsync(Product item)
        {
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    Product product = await GetItemAsync((int)item.Id);
                    if (product == null) throw new Exception("Товар не найден");
                    product.IdCategory = item.Category?.Id ?? item.IdCategory;
                    product.IdWarrantyPeriod = item.WarrantyPeriod?.Id ?? item.IdWarrantyPeriod;
                    product.IdUnitStorage = item.UnitStorage?.Id ?? item.IdUnitStorage;
                    product.IdPriceGroup = item.PriceGroup?.Id ?? item.IdPriceGroup;
                    product.Category = null;
                    product.WarrantyPeriod = null;
                    product.UnitStorage = null;
                    product.Barcode = item.Barcode;
                    product.Title = item.Title;
                    product.VendorCode = item.VendorCode;
                    product.PropertyProducts = null;
                    product.InvoiceInfos = null;
                    product.PriceProducts = null;
                    product.PriceGroup = null;
                    Db.Entry(product).State = EntityState.Modified;
                    await Db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        //public override async Task DeleteAsync(Product item)
        //{
        //    using (var transaction = Db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var product = await GetItemAsync(item.Id);
        //            if (product == null) throw new Exception("Товар не найден");
        //            Db.Entry(product).State = EntityState.Deleted;
        //            await Db.SaveChangesAsync();
        //            transaction.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Rollback();
        //            throw;
        //        }
        //    }
        //}
    }
}

using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.SalesGoods
{
    public class DbSetSalesGoods : AutomationAccountingGoodsEntities, IDbSetModel<SalesReports>
    {
        public async Task<ObservableCollection<SalesReports>> LoadAsync(int start, int end)
        {
            await SalesReports.Include(obj => obj.SalesInfos).OrderByDescending(obj => obj.DataSales)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end)
                .Include(obj => obj.Counterparties).Include(obj => obj.Stores).LoadAsync();
            return SalesReports.Local;
        }

        public async Task<int> GetCount()
        {
            return await PurchaseReports.CountAsync();
        }

        public async Task AddAsync(SalesReports obj)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    if (obj.Stores != null)
                    {
                        obj.IdStore = obj.Stores.Id;
                        obj.Stores = null;
                    }

                    foreach (var salesInfo in obj.SalesInfos)
                    {
                        if (salesInfo.Products != null)
                        {
                            salesInfo.IdProduct = salesInfo.Products.Id;
                            salesInfo.Products = null;
                        }

                        if (salesInfo.SerialNumbers != null && salesInfo.SerialNumbers.Id != 0)
                        {
                            salesInfo.IdSerialNumber = salesInfo.SerialNumbers.Id;
                            salesInfo.SerialNumbers = null;
                        }
                        else
                        {
                            salesInfo.IdSerialNumber = null;
                            salesInfo.SerialNumbers = null;
                        }
                    }
                    SalesReports.Add(obj);
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

        public async Task UpdateAsync(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

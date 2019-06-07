using System;
using System.Threading.Tasks;

namespace ModelModul.SalesGoods
{
    public class DbSetSalesGoods : AutomationAccountingGoodsEntities, IDbSetModel<SalesReports>
    {
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

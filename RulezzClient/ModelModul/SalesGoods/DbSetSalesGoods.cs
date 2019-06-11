using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.SalesGoods
{
    public class DbSetSalesGoods : IDbSetModel<SalesReports>
    {
        public ObservableCollection<SalesReports> Load(int start, int end)
        {
            return new ObservableCollection<SalesReports>(AutomationAccountingGoodsEntities.GetInstance().SalesReports.Include(obj => obj.SalesInfos).OrderByDescending(obj => obj.DataSales)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end)
                .Include(obj => obj.Counterparties).Include(obj => obj.Stores));
        }

        public int GetCount()
        {
            return AutomationAccountingGoodsEntities.GetInstance().PurchaseReports.Count();
        }

        public void Add(SalesReports obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
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
                    AutomationAccountingGoodsEntities.GetInstance().SalesReports.Add(obj);
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

        public void Update(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

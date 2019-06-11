using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.PurchaseGoods
{
    public class DbSetPurchaseGoods : IDbSetModel<PurchaseReports>
    {
        public ObservableCollection<PurchaseReports> Load(int start, int end)
        {
            return new ObservableCollection<PurchaseReports>(AutomationAccountingGoodsEntities.GetInstance()
                .PurchaseReports.Include(obj => obj.PurchaseInfos).OrderByDescending(obj => obj.DataOrder)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end).Include(obj => obj.Counterparties)
                .Include(obj => obj.Stores));
        }

        public int GetCount()
        {
            return AutomationAccountingGoodsEntities.GetInstance().PurchaseReports.Count();
        }

        public void Add(PurchaseReports obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    if (obj.Counterparties != null)
                    {
                        obj.IdCounterparty = obj.Counterparties.Id;
                        obj.Counterparties = null;
                    }
                    if (obj.Stores != null)
                    {
                        obj.IdStore = obj.Stores.Id;
                        obj.Stores = null;
                    }

                    List<SerialNumbers> serialNumbers = new List<SerialNumbers>();

                    foreach (var purchaseInfo in obj.PurchaseInfos)
                    {
                        if (purchaseInfo.Products != null)
                        {
                            purchaseInfo.IdProduct = purchaseInfo.Products.Id;
                            serialNumbers.AddRange(purchaseInfo.Products.SerialNumbers);
                            purchaseInfo.Products = null;
                        }

                        if (purchaseInfo.ExchangeRates != null)
                        {
                            purchaseInfo.IdExchangeRate = purchaseInfo.ExchangeRates.Id;
                            purchaseInfo.ExchangeRates = null;
                        }
                    }

                    foreach (var serialNumber in serialNumbers)
                    {
                        serialNumber.IdCounterparty = obj.IdCounterparty;
                        serialNumber.Counterparties = null;
                    }

                    AutomationAccountingGoodsEntities.GetInstance().PurchaseReports.Add(obj);

                    AutomationAccountingGoodsEntities.GetInstance().SerialNumbers.AddRange(serialNumbers);

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

        public void Update(PurchaseReports obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

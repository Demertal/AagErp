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
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                List<PurchaseReports> temp = new List<PurchaseReports>(db.PurchaseReports.Include(obj => obj.PurchaseInfos)
                    .OrderByDescending(obj => obj.DataOrder).ThenByDescending(obj => obj.Id).Skip(start).Take(end)
                    .Include(obj => obj.Counterparties).Include(obj => obj.Stores).ToList());
                temp.ForEach(obj => obj.PurchaseInfos.ToList().ForEach(objInfo => db.Entry(objInfo).Reference(objInf => objInf.Products).Load()));
                temp.ForEach(obj => obj.PurchaseInfos.ToList().ForEach(objInfo => db.Entry(objInfo).Reference(objInf => objInf.ExchangeRates).Load()));

                return new ObservableCollection<PurchaseReports>(temp);
            }
        }

        public int GetCount()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.PurchaseReports.Count();
            }
        }

        public void Add(PurchaseReports obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
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
                            serialNumbers.AddRange(purchaseInfo.SerialNumbers);
                            if (purchaseInfo.Products != null)
                            {
                                purchaseInfo.IdProduct = purchaseInfo.Products.Id;
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

                        db.PurchaseReports.Add(obj);
                        db.SerialNumbers.AddRange(serialNumbers);
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

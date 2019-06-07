using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.PurchaseGoods
{
    public class DbSetPurchaseGoods : AutomationAccountingGoodsEntities, IDbSetModel<PurchaseReports>
    {
        public async Task<ObservableCollection<PurchaseReports>> LoadAsync(int start, int end)
        {
            await PurchaseReports.Include(obj => obj.PurchaseInfos).OrderByDescending(obj => obj.DataOrder)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end)
                .Include(obj => obj.Counterparties).Include(obj => obj.Stores).LoadAsync();
            return PurchaseReports.Local;
        }

        public async Task<int> GetCount()
        {
            return await PurchaseReports.CountAsync();
        }

        public async Task AddAsync(PurchaseReports obj)
        {
            using (var transaction = Database.BeginTransaction())
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

                    PurchaseReports.Add(obj);

                    SerialNumbers.AddRange(serialNumbers);

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

        public async Task UpdateAsync(PurchaseReports obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

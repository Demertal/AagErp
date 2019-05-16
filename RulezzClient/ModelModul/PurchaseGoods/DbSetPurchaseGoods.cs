using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ModelModul.PurchaseGoods
{
    public class DbSetPurchaseGoods : DbSetModel<PurchaseReports>
    {
        public override ObservableCollection<PurchaseReports> List => null;

        public override async Task AddAsync(PurchaseReports obj)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(PurchaseReports purchaseReport, List<RevaluationProducts> revaluationProductses)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (purchaseReport.Suppliers != null)
                        {
                            purchaseReport.IdSupplier = purchaseReport.Suppliers.Id;
                            purchaseReport.Suppliers = null;
                        }
                        if (purchaseReport.Stores != null)
                        {
                            purchaseReport.IdStore = purchaseReport.Stores.Id;
                            purchaseReport.Stores = null;
                        }

                        List<SerialNumbers> serialNumbers = new List<SerialNumbers>();

                        foreach (var purchaseInfo in purchaseReport.PurchaseInfos)
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
                            serialNumber.IdSupplier = purchaseReport.IdSupplier;
                            serialNumber.Suppliers = null;
                        }

                        db.PurchaseReports.Add(purchaseReport);

                        db.SerialNumbers.AddRange(serialNumbers);

                        foreach (var revaluationProduct in revaluationProductses)
                        {
                            if (revaluationProduct.Products != null)
                            {
                                revaluationProduct.IdProduct = revaluationProduct.Products.Id;
                                revaluationProduct.Products = null;
                            }
                        }
                        db.RevaluationProducts.AddRange(revaluationProductses);
                        await db.SaveChangesAsync();
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

        public override async Task UpdateAsync(PurchaseReports obj)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}

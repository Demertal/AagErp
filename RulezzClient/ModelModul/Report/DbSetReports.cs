using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Documents;


namespace ModelModul.Report
{
    public class DbSetReports
    {
        public ObservableCollection<FinalReportProductViewModel> GetFinalReport(DateTime start, DateTime end)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                ObservableCollection<FinalReportProductViewModel> finalReport =
                    new ObservableCollection<FinalReportProductViewModel>();
                List<SalesReports> salesReports = db.SalesReports.Where(objSale => objSale.DataSales <= end)
                    .OrderBy(objSale => objSale.DataSales).ThenBy(objSale => objSale.Id).Include(objSale => objSale.SalesInfos).ToList();
                salesReports.Where(objSale => objSale.DataSales >= start).ToList().ForEach(objSale => objSale.SalesInfos
                    .ToList().ForEach(objIfo =>
                    {
                        if (finalReport.All(obj => obj.Product.Id != objIfo.IdProduct))
                            finalReport.Add(new FinalReportProductViewModel {Product = objIfo.Products});
                    }));
                List<PurchaseReports> purchaseReports = db.PurchaseReports
                    .Where(objPurRep => objPurRep.DataOrder <= end)
                    .OrderBy(objPurRep => objPurRep.DataOrder).ThenBy(objPurRep => objPurRep.Id)
                    .Include(objPurRep => objPurRep.PurchaseInfos).ToList();

                foreach (var report in finalReport)
                {
                    List<CountProducts> countProducts = report.Product.CountProducts.ToList();
                    foreach (var countProduct in countProducts)
                    {
                        int tempCountProduct = 0;

                        List<SalesReports> tempSalesReports =  salesReports.Where(obj =>
                            obj.IdStore == countProduct.IdStore &&
                            obj.SalesInfos.Any(objInf => objInf.IdProduct == countProduct.IdProduct)).ToList();

                        List<PurchaseReports> tempPurchaseReports = purchaseReports.Where(obj =>
                            obj.IdStore == countProduct.IdStore &&
                            obj.PurchaseInfos.Any(objInf => objInf.IdProduct == countProduct.IdProduct)).ToList();

                        if(tempSalesReports.Count == 0) continue;

                        tempSalesReports.ForEach(obj =>
                            obj.SalesInfos.Where(objInfo => objInfo.IdProduct == countProduct.IdProduct).ToList()
                                .ForEach(objInfo => tempCountProduct -= objInfo.Count));

                        tempPurchaseReports.ForEach(obj =>
                            obj.PurchaseInfos.Where(objInfo => objInfo.IdProduct == countProduct.IdProduct).ToList()
                                .ForEach(objInfo => tempCountProduct += objInfo.Count));

                        while (tempCountProduct < countProduct.Count)
                        {
                            PurchaseReports temp = db.PurchaseReports.First(obj =>
                                obj.DataOrder >= tempPurchaseReports.Last().DataOrder &&
                                obj.IdStore == countProduct.IdStore &&
                                obj.PurchaseInfos.Any(objInf => objInf.IdProduct == countProduct.IdProduct));

                            tempCountProduct += temp.PurchaseInfos.First(obj => obj.IdProduct == countProduct.IdProduct)
                                .Count;

                            tempPurchaseReports.Add(temp);
                        }

                        tempPurchaseReports.First().PurchaseInfos.First(obj => obj.IdProduct == countProduct.IdProduct)
                                .Count -= tempCountProduct - countProduct.Count;

                        tempSalesReports = tempSalesReports.Where(obj => obj.DataSales >= start).ToList();

                        while (tempSalesReports.Count != 0)
                        {
                           int count = tempSalesReports.First().SalesInfos
                                .Where(objInf => objInf.IdProduct == countProduct.IdProduct)
                                .Sum(objInf => objInf.Count);

                            report.Count += count;
                            report.FinalSum += count * (double) tempSalesReports.First().SalesInfos
                                                   .First(objInf => objInf.IdProduct == countProduct.IdProduct)
                                                   .SellingPrice;

                            foreach (var salesInfo in tempSalesReports.First().SalesInfos.Where(objInf => objInf.IdProduct == countProduct.IdProduct))
                            {
                                if (salesInfo.SerialNumbers == null) continue;
                                if (salesInfo.SerialNumbers.PurchaseInfos.ExchangeRates.Title == "USD")
                                {
                                    report.FinalSum -=
                                        (double)salesInfo.SerialNumbers.PurchaseInfos.PurchasePrice *
                                        (double)salesInfo.SerialNumbers.PurchaseInfos.PurchaseReports.Course;
                                }
                                else
                                {
                                    report.FinalSum -= (double) salesInfo.SerialNumbers.PurchaseInfos.PurchasePrice;
                                }

                                count--;
                            }

                            while (count != 0)
                            {
                                double purchasePrice = (double)tempPurchaseReports.First().PurchaseInfos
                                    .First(objInf => objInf.IdProduct == countProduct.IdProduct)
                                    .PurchasePrice;

                                if (tempPurchaseReports.First().PurchaseInfos
                                        .First(objInf => objInf.IdProduct == countProduct.IdProduct).ExchangeRates
                                        .Title == "USD")
                                {
                                    purchasePrice *= (double)tempPurchaseReports.First().Course;
                                }

                                if (count <= tempPurchaseReports.First().PurchaseInfos
                                        .First(objInf => objInf.IdProduct == countProduct.IdProduct).Count)
                                {
                                    report.FinalSum -= count * purchasePrice;
                                    tempPurchaseReports.First().PurchaseInfos.First(objInf =>
                                        objInf.IdProduct == countProduct.IdProduct).Count -= count;
                                    count = 0;
                                }
                                else
                                {
                                    report.FinalSum -=
                                        tempPurchaseReports.First().PurchaseInfos.First(objInf =>
                                            objInf.IdProduct == countProduct.IdProduct).Count * purchasePrice;
                                    count -= tempPurchaseReports.First().PurchaseInfos
                                        .First(objInf => objInf.IdProduct == countProduct.IdProduct).Count;
                                    tempPurchaseReports.First().PurchaseInfos
                                        .First(objInf => objInf.IdProduct == countProduct.IdProduct).Count = 0;
                                }

                                if (tempPurchaseReports.First().PurchaseInfos
                                        .First(objInf => objInf.IdProduct == countProduct.IdProduct).Count ==
                                    0) tempPurchaseReports.RemoveAt(0);
                            }

                            tempSalesReports.RemoveAt(0);
                        }
                    }
                }
                return finalReport;
            }
        }
    }
}

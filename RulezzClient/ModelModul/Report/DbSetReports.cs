using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.Report
{
    public class DbSetReports
    {
        public ObservableCollection<FinalReportProductViewModel> GetFinalReport(DateTime start, DateTime end)
        {
            ObservableCollection<FinalReportProductViewModel> finalReport = new ObservableCollection<FinalReportProductViewModel>();
            List<SalesReports> tempSales = AutomationAccountingGoodsEntities.GetInstance().SalesReports.Where(objSale => objSale.DataSales >= start && objSale.DataSales <= end)
                .Include(objSale => objSale.SalesInfos).ToList();
            foreach (var sales in tempSales)
            {
                foreach (var info in sales.SalesInfos)
                {
                    finalReport.Add(new FinalReportProductViewModel());
                    finalReport[finalReport.Count - 1].Product = info.Products;
                    finalReport[finalReport.Count - 1].Count += info.Count;
                }
            }
            AutomationAccountingGoodsEntities.GetInstance().PurchaseReports.Where(objPur => objPur.DataOrder >= start && objPur.DataOrder <= end)
                .Include(objPur => objPur.PurchaseInfos).Load();
            List<PurchaseReports> tempPur = AutomationAccountingGoodsEntities.GetInstance().PurchaseReports.Local.ToList();
            return null;
        }
    }
}

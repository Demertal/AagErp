using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ModelModul.Product;

namespace ModelModul.Report
{
    public class DbSetReports: AutomationAccountingGoodsEntities
    {
        public async Task<ObservableCollection<FinalReportProductViewModel>> GetFinalReport(DateTime start, DateTime end)
        {
            ObservableCollection<FinalReportProductViewModel> finalReport = new ObservableCollection<FinalReportProductViewModel>();
            List<SalesReports> tempSales = await SalesReports.Where(objSale => objSale.DataSales >= start && objSale.DataSales <= end)
                .Include(objSale => objSale.SalesInfos).ToListAsync();
            foreach (var sales in tempSales)
            {
                foreach (var info in sales.SalesInfos)
                {
                    finalReport.Add(new FinalReportProductViewModel());
                    finalReport[finalReport.Count - 1].Product = info.Products;
                    finalReport[finalReport.Count - 1].Count += info.Count;
                }
            }
            await PurchaseReports.Where(objPur => objPur.DataOrder >= start && objPur.DataOrder <= end)
                .Include(objPur => objPur.PurchaseInfos).LoadAsync();
            List<PurchaseReports> tempPur = PurchaseReports.Local.ToList();
            return null;
        }
    }
}

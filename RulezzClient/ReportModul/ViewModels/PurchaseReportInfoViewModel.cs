using System.Linq;
using ModelModul;
using Prism.Mvvm;

namespace ReportModul.ViewModels
{
    class PurchaseReportInfoViewModel : BindableBase
    {
        #region Properties


        private PurchaseReports _purchaseReport = new PurchaseReports();
        public PurchaseReports PurchaseReport
        {
            get => _purchaseReport;
            set => SetProperty(ref _purchaseReport, value);
        }

        #endregion

        public PurchaseReportInfoViewModel()
        {
        }
    }
}

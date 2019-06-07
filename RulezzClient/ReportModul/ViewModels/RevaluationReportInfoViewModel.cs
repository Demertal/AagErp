using ModelModul;
using Prism.Mvvm;

namespace ReportModul.ViewModels
{
    class RevaluationReportInfoViewModel: BindableBase
    {
        #region Properties


        private RevaluationProductsReports _revaluationProductsReport = new RevaluationProductsReports();
        public RevaluationProductsReports RevaluationProductsReport
        {
            get => _revaluationProductsReport;
            set => SetProperty(ref _revaluationProductsReport, value);
        }

        #endregion

        public RevaluationReportInfoViewModel()
        {
        }
    }
}

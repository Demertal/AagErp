using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ReportModul.Views;

namespace ReportModul
{
    public class ReportModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("RevaluationReportInfo", typeof(RevaluationReportInfo));
            regionManager.RegisterViewWithRegion("PurchaseReportInfo", typeof(PurchaseReportInfo));
            regionManager.RegisterViewWithRegion("SalesReportInfo", typeof(SalesReportInfo));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowFinalReport>();
            containerRegistry.RegisterForNavigation<ShowRevaluationReports>();
            containerRegistry.RegisterForNavigation<ShowPurchaseReports>();
            containerRegistry.RegisterForNavigation<ShowSalesReports>();
        }
    }
}
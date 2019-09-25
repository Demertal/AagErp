using Prism.Ioc;
using Prism.Modularity;
using ReportModul.Views;

namespace ReportModul
{
    public class ReportModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowProfitStatement>();
            containerRegistry.RegisterForNavigation<ShowRevaluationReports>();
            containerRegistry.RegisterForNavigation<ShowPurchaseReports>();
            containerRegistry.RegisterForNavigation<ShowSalesReports>();
            containerRegistry.RegisterForNavigation<ShowTransportationGoodsReports>();
        }
    }
}
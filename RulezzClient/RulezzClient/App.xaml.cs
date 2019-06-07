using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RulezzClient.Views;
using UnitStorageModul;

namespace RulezzClient
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ProductModul.ProductModul>();
            moduleCatalog.AddModule<PurchaseGoodsModul.PurchaseGoodsModul>();
            moduleCatalog.AddModule<PropertiesModul.PropertiesModul>();
            moduleCatalog.AddModule<CounterpartyModul.CounterpartiesModul>();
            moduleCatalog.AddModule<RevaluationProductsModul.RevaluationProductsModul>();
            moduleCatalog.AddModule<UnitStoragesModul>();
            moduleCatalog.AddModule<WarrantyPeriodsModul.WarrantyPeriodsModul>();
            moduleCatalog.AddModule<CashierWorkplaceModul.CashierWorkplaceModul>();
            moduleCatalog.AddModule<WarrantyModul.WarrantyModul>();
            moduleCatalog.AddModule<ReportModul.ReportModul>();
        }
    }
}

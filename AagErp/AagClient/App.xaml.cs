using System.Windows;
using AagClient.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Shell = AagClient.Views.Shell;

namespace AagClient
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return new Shell();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Authorization>();
            containerRegistry.RegisterForNavigation<Workspace>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ProductModul.ProductModul>();
            moduleCatalog.AddModule<UnitStorageModul.UnitStorageModul>();
            moduleCatalog.AddModule<WarrantyPeriodModul.WarrantyPeriodModul>();
            moduleCatalog.AddModule<PriceGroupModul.PriceGroupModul>();
            moduleCatalog.AddModule<StoreModul.StoreModul>();
            moduleCatalog.AddModule<CurrencyModul.CurrencyModul>();
            moduleCatalog.AddModule<CounterpartyModul.CounterpartyModul>();
            moduleCatalog.AddModule<RevaluationGoodModul.RevaluationGoodModul>();
            moduleCatalog.AddModule<PurchaseGoodModul.PurchaseGoodModul>();
            moduleCatalog.AddModule<CashierWorkplaceModul.CashierWorkplaceModul>();
            moduleCatalog.AddModule<TransportationGoods.TransportationGoodsModul>();
            moduleCatalog.AddModule<PropertyModul.PropertyModul>();
            moduleCatalog.AddModule<ReportModul.ReportModul>();
            //moduleCatalog.AddModule<WarrantyModul.WarrantyModul>();
        }
    }
}

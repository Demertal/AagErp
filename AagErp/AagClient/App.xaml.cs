using System.Windows;
using AagClient.ViewModels;
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
            Window shell = null;
            Window loginWindow = new Authorization();
            ((AuthorizationViewModel)loginWindow.DataContext).LoginCompleted += ev =>
            {
                shell = Container.Resolve<Shell>();
                loginWindow.Hide();
                loginWindow.Close();
            };
            loginWindow.ShowDialog();
            if (shell != null) return shell;
            Current.Shutdown();
            return new Window();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
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
            //moduleCatalog.AddModule<WarrantyModul.WarrantyModul>();
            //moduleCatalog.AddModule<ReportModul.ReportModul>();
        }
    }
}

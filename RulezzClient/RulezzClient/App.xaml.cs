using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RulezzClient.ViewModels;
using RulezzClient.Views;

namespace RulezzClient
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
            //moduleCatalog.AddModule<PurchaseGoodsModul.PurchaseGoodsModul>();
            //moduleCatalog.AddModule<PropertiesModul.PropertiesModul>();
            //moduleCatalog.AddModule<CounterpartiesModul>();
            //moduleCatalog.AddModule<RevaluationProductsModul.RevaluationProductsModul>();
            moduleCatalog.AddModule<UnitStorageModul.UnitStorageModul>();
            moduleCatalog.AddModule<WarrantyPeriodModul.WarrantyPeriodModul>();
            moduleCatalog.AddModule<PriceGroupModul.PriceGroupModul>();
            //moduleCatalog.AddModule<CashierWorkplaceModul.CashierWorkplaceModul>();
            //moduleCatalog.AddModule<WarrantyModul.WarrantyModul>();
            //moduleCatalog.AddModule<ReportModul.ReportModul>();
        }
    }
}

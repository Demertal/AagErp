using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RulezzClient.Views;

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
            moduleCatalog.AddModule<ProductModul.ShowProductModul>();
            moduleCatalog.AddModule<GroupModul.GroupModul>();
        }
    }
}

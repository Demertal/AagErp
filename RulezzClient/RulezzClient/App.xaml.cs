using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RulezzClient.Views;

namespace RulezzClient
{
    internal partial class App : PrismApplication
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    Bootstrapper bootstrapper = new Bootstrapper();
        //    bootstrapper.Run();
        //}
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new System.NotImplementedException();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}

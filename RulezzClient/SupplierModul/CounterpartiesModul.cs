using CounterpartyModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CounterpartyModul
{
    public class CounterpartiesModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("CounterpartyInfo", typeof(CounterpartyInfo));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowCounterparties>();
        }
    }
}

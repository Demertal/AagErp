using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using WarrantyModul.Views;

namespace WarrantyModul
{
    public class WarrantyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("WarrantyInfo", typeof(WarrantyInfo));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowWarranties>();
        }
    }
}

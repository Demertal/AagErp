using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ProductModul.Views;

namespace ProductModul
{
    public class ProductModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ShowProduct));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}

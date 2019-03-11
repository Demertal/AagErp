using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ShowProduct = ProductModul.Views.ShowProduct;

namespace ProductModul
{
    public class ShowProductModul : IModule
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

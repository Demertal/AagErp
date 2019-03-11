using GroupModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GroupModul
{
    public class GroupModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ShowGroupRegion", typeof(ShowGroup));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}

using Prism.Ioc;
using Prism.Modularity;
using StoreModul.Views;

namespace StoreModul
{
    public class StoreModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowStores>();
        }
    }
}

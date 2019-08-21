using Prism.Ioc;
using Prism.Modularity;
using PropertiesModul.Views;

namespace PropertiesModul
{
    public class PropertiesModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowProperties>();
        }
    }
}

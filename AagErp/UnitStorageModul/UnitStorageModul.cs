using Prism.Ioc;
using Prism.Modularity;
using UnitStorageModul.Views;

namespace UnitStorageModul
{
    public class UnitStorageModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowUnitStorages>();
        }
    }
}
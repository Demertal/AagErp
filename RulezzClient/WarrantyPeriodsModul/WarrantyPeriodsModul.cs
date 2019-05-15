using Prism.Ioc;
using Prism.Modularity;
using WarrantyPeriodsModul.Views;

namespace WarrantyPeriodsModul
{
    public class WarrantyPeriodsModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowWarrantyPeriods>();
        }
    }
}

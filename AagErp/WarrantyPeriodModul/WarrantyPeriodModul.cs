using Prism.Ioc;
using Prism.Modularity;
using ShowWarrantyPeriods = WarrantyPeriodModul.Views.ShowWarrantyPeriods;

namespace WarrantyPeriodModul
{
    public class WarrantyPeriodModul : IModule
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

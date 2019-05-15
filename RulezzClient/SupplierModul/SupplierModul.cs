using Prism.Ioc;
using Prism.Modularity;
using SupplierModul.Views;

namespace SupplierModul
{
    public class SupplierModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowSuppliers>();
        }
    }
}

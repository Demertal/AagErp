using CurrencyModul.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace CurrencyModul
{
    public class CurrencyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowCurrencies>();
        }
    }
}

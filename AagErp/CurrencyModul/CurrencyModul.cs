using CurrencyModul.ViewModels;
using CurrencyModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using DialogWindow = ModelModul.MVVM.DialogWindow;

namespace CurrencyModul
{
    public class CurrencyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterForNavigation<ShowCurrencies>();
            containerRegistry.RegisterDialog<ShowCurrency, ShowCurrencyViewModel>();
        }
    }
}

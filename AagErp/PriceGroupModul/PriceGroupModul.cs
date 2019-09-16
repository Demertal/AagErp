using ModelModul.MVVM;
using PriceGroupModul.ViewModels;
using PriceGroupModul.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace PriceGroupModul
{
    public class PriceGroupModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowPriceGroups>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<ShowPriceGroup, ShowPriceGroupViewModel>();
        }
    }
}
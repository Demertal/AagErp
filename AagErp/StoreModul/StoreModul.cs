using ModelModul.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using StoreModul.ViewModels;
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
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<ShowStore, ShowStoreViewModel>();
        }
    }
}

using CashierWorkplaceModul.Views;
using CustomControlLibrary.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;

namespace CashierWorkplaceModul
{
    public class CashierWorkplaceModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CashierWorkplace>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<Catalog, СatalogViewModel>();
        }
    }
}

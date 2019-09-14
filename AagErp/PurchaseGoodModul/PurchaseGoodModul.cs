using CustomControlLibrary.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using PurchaseGoodModul.Views;

namespace PurchaseGoodModul
{
    public class PurchaseGoodModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PurchaseGood>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<Catalog, СatalogViewModel>();
        }
    }
}
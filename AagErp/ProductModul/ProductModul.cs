using CategoryModul.ViewModels;
using CategoryModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using DialogWindow = ModelModul.MVVM.DialogWindow;

namespace ProductModul
{
    public class ProductModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterForNavigation<Catalog>();
            containerRegistry.RegisterForNavigation<ShowNomenclature>();
            containerRegistry.RegisterDialog<ShowCategory, ShowCategoryViewModel>();
            containerRegistry.RegisterDialog<ShowProduct, ShowProductViewModel>();
        }
    }
}

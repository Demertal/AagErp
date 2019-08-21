using CategoryModul.ViewModels;
using CustomControlLibrary.MVVM;
using GroupModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ProductModul.ViewModels;
using ProductModul.Views;

namespace ProductModul
{
    public class ProductModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ProductInfo", typeof(ProductInfo));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterForNavigation<ShowProduct>();
            containerRegistry.RegisterDialog<AddCategory, AddCategoryViewModel>();
            containerRegistry.RegisterDialog<RenameCategory, RenameCategoryViewModel>();
            containerRegistry.RegisterDialog<AddProduct, AddProductViewModel>();
        }
    }
}

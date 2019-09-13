using CategoryModul.ViewModels;
using CustomControlLibrary.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using AddCategory = CategoryModul.Views.AddCategory;
using RenameCategory = CategoryModul.Views.RenameCategory;

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
            containerRegistry.RegisterForNavigation<ShowProduct>();
            containerRegistry.RegisterForNavigation<ShowNomenclature>();
            containerRegistry.RegisterDialog<AddCategory, AddCategoryViewModel>();
            containerRegistry.RegisterDialog<RenameCategory, RenameCategoryViewModel>();
            containerRegistry.RegisterDialog<AddProduct, AddProductViewModel>();
            containerRegistry.RegisterDialog<ProductInfo, ProductInfoViewModel>();
        }
    }
}

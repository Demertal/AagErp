using CategoryModul.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using AddCategory = CategoryModul.Views.AddCategory;
using DialogWindow = ModelModul.MVVM.DialogWindow;
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
            containerRegistry.RegisterForNavigation<Catalog>();
            containerRegistry.RegisterForNavigation<ShowNomenclature>();
            containerRegistry.RegisterDialog<AddCategory, AddCategoryViewModel>();
            containerRegistry.RegisterDialog<RenameCategory, RenameCategoryViewModel>();
            containerRegistry.RegisterDialog<ShowProduct, ShowProductViewModel>();
        }
    }
}

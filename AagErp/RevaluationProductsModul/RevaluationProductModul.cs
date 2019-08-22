using CustomControlLibrary.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using RevaluationProductModul.Views;

namespace RevaluationProductModul
{
    public class RevaluationProductModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RevaluationProduct>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<ShowProduct, ShowProductViewModel>();
        }
    }
}

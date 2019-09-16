using ModelModul.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using UnitStorageModul.ViewModels;
using UnitStorageModul.Views;

namespace UnitStorageModul
{
    public class UnitStorageModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterForNavigation<ShowUnitStorages>();
            containerRegistry.RegisterDialog<ShowUnitStorage, ShowUnitStorageViewModel>();
        }
    }
}
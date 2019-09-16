using Prism.Ioc;
using Prism.Modularity;
using WarrantyPeriodModul.ViewModels;
using WarrantyPeriodModul.Views;
using DialogWindow = ModelModul.MVVM.DialogWindow;
using ShowWarrantyPeriods = WarrantyPeriodModul.Views.ShowWarrantyPeriods;

namespace WarrantyPeriodModul
{
    public class WarrantyPeriodModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterForNavigation<ShowWarrantyPeriods>();
            containerRegistry.RegisterDialog<ShowWarrantyPeriod, ShowWarrantyPeriodViewModel>();
        }
    }
}

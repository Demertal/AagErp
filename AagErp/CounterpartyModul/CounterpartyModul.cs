using CounterpartyModul.ViewModels;
using CounterpartyModul.Views;
using Prism.Ioc;
using Prism.Modularity;
using DialogWindow = ModelModul.MVVM.DialogWindow;

namespace CounterpartyModul
{
    public class CounterpartyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowCounterparties>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<ShowCounterparty, ShowCounterpartyViewModel>();
        }
    }
}

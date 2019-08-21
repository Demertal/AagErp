using CounterpartyModul.ViewModels;
using CounterpartyModul.Views;
using CustomControlLibrary.MVVM;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CounterpartyModul
{
    public class CounterpartyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("CounterpartyInfo", typeof(CounterpartyInfo));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowCounterparties>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<AddCounterparty, AddCounterpartyViewModel>();
        }
    }
}

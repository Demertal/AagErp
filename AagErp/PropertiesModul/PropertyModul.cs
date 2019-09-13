using Prism.Ioc;
using Prism.Modularity;
using PropertyModul.ViewModels;
using PropertyModul.Views;
using ShowProperties = PropertyModul.Views.ShowProperties;

namespace PropertyModul
{
    public class PropertyModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ShowProperties>();
            containerRegistry.RegisterDialog<ShowProperty, ShowPropertyViewModel>();
        }
    }
}

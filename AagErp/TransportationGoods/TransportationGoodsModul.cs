using Prism.Ioc;
using Prism.Modularity;
using ProductModul.ViewModels;
using ProductModul.Views;
using DialogWindow = ModelModul.MVVM.DialogWindow;

namespace TransportationGoods
{
    public class TransportationGoodsModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.TransportationGoods>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<Catalog, СatalogViewModel>();
        }
    }
}

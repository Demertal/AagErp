using Prism.Ioc;
using Prism.Modularity;
using PurchaseGoodsModul.Views;

namespace PurchaseGoodsModul
{
    public class PurchaseGoodsModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PurchaseGoods>();
        }
    }
}
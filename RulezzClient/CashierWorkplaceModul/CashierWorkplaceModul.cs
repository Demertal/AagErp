using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierWorkplaceModul.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace CashierWorkplaceModul
{
    public class CashierWorkplaceModul : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CashierWorkplace>();
        }
    }
}

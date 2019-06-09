using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using ModelModul;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace RulezzClient.ViewModels
{
    class ShellViewModel : BindableBase
    {
        #region Parametr

        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; }
        #endregion

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath == null) return;
            if(navigatePath == "ShowCounterparties")
                _regionManager.RequestNavigate("ContentRegion", "ShowCounterparties", new NavigationParameters{
                    {"type", TypeCounterparties.Suppliers}});
            else if(navigatePath == "ShowCustomers")
                _regionManager.RequestNavigate("ContentRegion", "ShowCounterparties", new NavigationParameters{
                    {"type", TypeCounterparties.Buyers}});
            else
                _regionManager.RequestNavigate("ContentRegion", navigatePath, new NavigationParameters());
        }
    }
}
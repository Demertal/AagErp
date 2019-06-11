using ModelModul;
using Prism.Commands;
using Prism.Regions;

namespace RulezzClient.ViewModels
{
    class ShellViewModel : ViewModelBase
    {
        #region Parametr

        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; }
        #endregion

        public ShellViewModel(IRegionManager regionManager)
        {
            //AutomationAccountingGoodsEntities au = new AutomationAccountingGoodsEntities();
            //au.Database.Connection.Open();
            //var t = au.Database.SqlQuery<int>("SELECT Is_Member (\'Seller\')").ToList();
           
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath == null) return;
            NavigationParameters navigation = new NavigationParameters();
            if (navigatePath == "ShowSuppliers")
            {
                navigation.Add("type", TypeCounterparties.Suppliers);
                navigatePath = "ShowCounterparties";
            }
            else if(navigatePath == "ShowCustomers")
            {
                navigation.Add("type", TypeCounterparties.Buyers);
                navigatePath = "ShowCounterparties";
            }
            _regionManager.RequestNavigate("ContentRegion", navigatePath, navigation);
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
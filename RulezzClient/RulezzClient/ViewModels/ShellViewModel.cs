using CustomControlLibrary.MVVM;
using ModelModul.Models;
using Prism.Commands;
using Prism.Regions;

namespace RulezzClient.ViewModels
{
    enum Roles { Seller, OldestSalesman, Admin}

    class ShellViewModel : ViewModelBase
    {
        #region Parametr

        private readonly IRegionManager _regionManager;

        private Roles _role;

        public Roles Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand<string> NavigateCommand { get; }
        #endregion

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Role = Roles.Admin;
            //using (AutomationAccountingGoodsContext db =
            //    new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
            //{
            //    var result = db.Database.SqlQuery<int?>("SELECT is_member ('Seller')").ToList();
            //    if (result[0] == 1) Role = Roles.Seller;
            //    else
            //    {
            //        result = db.Database.SqlQuery<int?>("SELECT is_member ('OldestSalesman')").ToList();
            //        if (result[0] == 1) Role = Roles.OldestSalesman;
            //        else
            //        {
            //            result =db.Database.SqlQuery<int?>("SELECT is_member ('Admin')").ToList();
            //            if (result[0] == 1) Role = Roles.Admin;
            //        }
            //    }
            //}

            LoadedCommand = new DelegateCommand(Loaded);
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Loaded()
        {
            if (Role == Roles.Seller)
            {
                Navigate("CashierWorkplace");
            }
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
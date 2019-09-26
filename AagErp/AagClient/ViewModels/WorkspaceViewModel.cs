using ModelModul;
using ModelModul.Models;
using ModelModul.MVVM;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AagClient.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        #region Parametr

        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private Roles _role;
        public Roles Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }
        public string UserInfo
        {
            get
            {
                if (ConnectionTools.CurrrentUser == null) return "";
                string temp = "Логин: " + ConnectionTools.CurrrentUser?.Login + "; Роль: ";
                switch (Role)
                {
                    case Roles.Seller:
                        if(ConnectionTools.CurrrentUser.Store == null) return "";
                        temp += "Продавец; Склад: " + ConnectionTools.CurrrentUser?.Store.Title;
                        break;
                    case Roles.Storekeeper:
                        temp += "Кладовщик";
                        break;
                    case Roles.Manager:
                        temp += "Менеджер";
                        break;
                }
                return temp;
            }
        }

        public DelegateCommand<string> NavigateCommand { get; }

        #endregion

        public WorkspaceViewModel(IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(dialogService)
        {
            
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EventRole>().Subscribe(e =>
            {
                Role = e;
                RaisePropertyChanged(nameof(UserInfo));
            });
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath == null) return;
            NavigationParameters navigation = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegionWorkspace", navigatePath, navigation);
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

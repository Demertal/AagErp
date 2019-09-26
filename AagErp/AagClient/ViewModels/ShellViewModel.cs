using System.Windows;
using Microsoft.EntityFrameworkCore;
using ModelModul;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AagClient.ViewModels
{
    public enum Roles { Seller, Storekeeper, Manager }

    class ShellViewModel : ViewModelBase
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

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        private SizeToContent _sizeToContent;
        public SizeToContent SizeToContent
        {
            get => _sizeToContent;
            set => SetProperty(ref _sizeToContent, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand LoadedCommand { get; }
        #endregion

        public ShellViewModel(IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator) : base(dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EventBool>().Subscribe(e =>
            {
                if (!e) return;

                using (AutomationAccountingGoodsContext db = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString))
                {
                    var conn = db.Database.GetDbConnection();
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "SELECT IS_ROLEMEMBER ('Seller')";
                    var reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.GetInt32(0) == 1)
                    {
                        Role = Roles.Seller;
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        command.CommandText = "SELECT IS_ROLEMEMBER ('Storekeeper')";
                        reader = command.ExecuteReader();
                        reader.Read();
                        if (reader.GetInt32(0) == 1)
                        {
                            Role = Roles.Storekeeper;
                            reader.Close();
                        }
                        else
                        {
                            reader.Close();
                            command.CommandText = "SELECT IS_ROLEMEMBER ('Manager')";
                            reader = command.ExecuteReader();
                            reader.Read();
                            if (reader.GetInt32(0) == 1)
                            {
                                Role = Roles.Manager;
                                reader.Close();
                            }
                            else
                            {
                                reader.Close();
                                WindowState = WindowState.Normal;
                                SizeToContent = SizeToContent.WidthAndHeight;
                                Title = "Авторизация";
                                _regionManager.RequestNavigate("ContentRegionShell", "Authorization", new NavigationParameters());
                            }
                            
                        }
                    }
                }

                LoadUser();
            });
            WindowState = WindowState.Normal;
            SizeToContent = SizeToContent.WidthAndHeight;
            Title = "Авторизация";
            LoadedCommand = new DelegateCommand(Loaded);
        }

        private async void LoadUser()
        {
            IRepository<User> userRepository = new SqlUserRepository();
            ConnectionTools.CurrrentUser = await userRepository.GetItemAsync(
                where: new ExpressionSpecification<User>(u => u.Login == ConnectionTools.Login),
                include: (u => u.Store, null));
            _regionManager.RequestNavigate("ContentRegionShell", "Workspace", new NavigationParameters());
            _eventAggregator.GetEvent<EventRole>().Publish(Role);
            SizeToContent = SizeToContent.Manual;
            WindowState = WindowState.Maximized;
            Title = "Главное окно";
        }

        private void Loaded()
        {
            _regionManager.RequestNavigate("ContentRegionShell", "Authorization", new NavigationParameters());
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
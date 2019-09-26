using System;
using System.IO;
using System.Windows.Forms;
using ModelModul;
using ModelModul.MVVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AagClient.ViewModels
{
    class AuthorizationViewModel : ViewModelBase
    {
        #region Properties

        private readonly IEventAggregator _eventAggregator;

        private string _login;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DelegateCommand EnterCommand { get; }
        
        #endregion

        public AuthorizationViewModel(IDialogService dialogService, IEventAggregator eventAggregator) : base(dialogService)
        {
            _eventAggregator = eventAggregator;
            EnterCommand = new DelegateCommand(Enter);
        }

        private async void Enter()
        {
            try
            {
                string dataSource;
                using (StreamReader r = new StreamReader("appsettings.json"))
                {
                    var json = r.ReadToEnd();
                    var data = (JObject)JsonConvert.DeserializeObject(json);
                    var сonnectionStrings = data["ConnectionStrings"].Value<JObject>();
                    dataSource = сonnectionStrings["Server"].Value<string>();
                }

                ConnectionTools.Login = Login;
                ConnectionTools.BuildConnectionString("AutomationAccountingGoodsContext", dataSource: dataSource, userId:Login, password:Password, integratedSecuity: false);
                AutomationAccountingGoodsContext au = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString);
                if (await au.Database.CanConnectAsync())
                    _eventAggregator.GetEvent<EventBool>().Publish(true);
                else throw new Exception("Ошибка соединения");
            }
            catch (Exception e)
            {
                ConnectionTools.Login = string.Empty;
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConnectionTools.Login = string.Empty;
            ConnectionTools.CurrrentUser = null;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}

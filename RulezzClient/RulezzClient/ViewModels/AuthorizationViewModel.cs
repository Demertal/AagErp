using System;
using System.IO;
using System.Windows.Forms;
using ModelModul;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Regions;

namespace RulezzClient.ViewModels
{
    class AuthorizationViewModel : ViewModelBase
    {
        #region Properties

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

        public delegate void LoginEvent(bool completed);
        public event LoginEvent LoginCompleted;

        #endregion

        public AuthorizationViewModel()
        {
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

                ConnectionTools.BuildConnectionString("AutomationAccountingGoodsContext");
                AutomationAccountingGoodsContext au = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString);
                //await au.Database.Connection.OpenAsync();
                LoginCompleted?.Invoke(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
    }
}

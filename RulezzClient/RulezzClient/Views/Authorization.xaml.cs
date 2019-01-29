using System;
using System.Windows;

namespace RulezzClient.Views
{
    public partial class Authorization
    {
        //TODO доделать окно админ клиента
        //private string _loginAdminKlient = "AdminKlient";
        //private string _passwordAdminKlient = "AdminKlient";
        bool _loginSuccess;
        //private readonly SqlConnection _connection;
        //private readonly MainWindow _mw;

        public Authorization()
        {
            InitializeComponent();
            _loginSuccess = false;
            //_mw = mw;
            //GetIP_Port();
                        // Создание подключения
          //  _connection = new SqlConnection(Settings.Default.СconnectionString);
            //try
            //{
            //    // Открываем подключение
            //    _connection.Open();
            //    status_conect_ellipse.Fill = new SolidColorBrush(Colors.Green);
            //}
            //catch
            //{
            //    // закрываем подключение
            //    _connection.Close();
            //}

            //TODO удалить потом
            LoginTextBox.Text = "u1";
            PasswordPasswordBox.Password = "u1";
            //ReadUsersAsync().GetAwaiter();
        }

        //TODO переделать на чтение из файла
        //private void GetIP_Port()
        //{
        //    try
        //    {
        //        string[] findDirectory = Directory.GetDirectories(@"C:\Program Files", "RulezzClient", SearchOption.TopDirectoryOnly);
        //        if (findDirectory.Length == 0)
        //        {
        //            Directory.CreateDirectory(@"C:\Program Files\RulezzClient");
        //        }
        //        else
        //        {
        //            string[] findFile = Directory.GetFiles(@"C:\Program Files\RulezzClient", "settings", SearchOption.TopDirectoryOnly);
        //            if (findFile.Length == 0)
        //            {
        //                File.Create(@"C:\Program Files\RulezzClient\settings");
        //            }
        //            else
        //            {
        //                using (StreamReader fs = new StreamReader(findFile[0]))
        //                {
        //                    _ipServer = fs.ReadLine();
        //                    _portServer = fs.ReadLine();
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ReadUsersAsync().GetAwaiter();
        }

        //private async Task ReadUsersAsync()
        //{
        //    const string nameComm = "select * from UserAuthorization(@name,@password)";
        //   // SqlCommand command = new SqlCommand(nameComm, _connection);
        //    //command.Parameters.AddWithValue("@name", LoginTextBox.Text);
        //   // command.Parameters.AddWithValue("@password", PasswordPasswordBox.Password);
        //    //SqlDataReader reader = await command.ExecuteReaderAsync();

        //    //if (reader.HasRows)
        //    //{
        //    //    while (reader.Read()) // построчно считываем данные
        //    //    {
        //    //        //_mw.IdStore = (int)reader.GetValue(0);
        //    //        //_mw.IdRole = (int)reader.GetValue(1);
        //    //        _loginSuccess = true;
        //    //        _connection.Close();
        //    //        reader.Close();
        //    //        Close();
        //    //    }
        //    //}
        //    //else
        //    //    MessageBox.Show("Неверный логин или пароль.", "Ошибка аторизации.", MessageBoxButton.OK,
        //    //        MessageBoxImage.Error);
        //    //reader.Close();
        //}

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!_loginSuccess) Application.Current.Shutdown();
        }
    }
}

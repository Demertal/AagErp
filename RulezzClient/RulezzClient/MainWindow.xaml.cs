using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace RulezzClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow
    {
        //TODO переделать на чтение из файла
        //string ip_server;
        //string port_server;
        //TODO переделать на создание строки из разных параметров
        public string NameServer = "";
        public string ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += Window1_SourceInitialized;
            //Form1 f = new Form1(this);
            //f.ShowDialog();
            ConnectionString =
                $"Data Source=;Initial Catalog=Rul_base;Integrated Security=true";
            Authorization au = new Authorization(this);
            au.ShowDialog();
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            connection.Close();
            DataContext = new MainViewVm(Dispatcher.CurrentDispatcher, ConnectionString);
        }

        private void MiAddProduct_Click(object sender, RoutedEventArgs e)
        {
            //AddProduct ad = new AddProduct(this);
            //ad.ShowDialog();
        }

        private void MIChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            //Product product = (Product) DgProducts.SelectedItem;
            //ProductDataContext db = new ProductDataContext(ConnectionString);
            ////db.FindProductId(product.Title, _selectedNomenclatureSubgroup, ref id);
            //AddProduct ad = new AddProduct(this, id);
            //ad.ShowDialog();
            //ShowFunctionAsync(3);
        }

        private void MiDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить товар?", "Удаление", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                //int id = 0;
                //Product product = (Product) DgProducts.SelectedItem;
                //ProductDataContext db = new ProductDataContext(ConnectionString);
                //try
                //{
                //    db.Connection.Open();
                //    db.Transaction = db.Connection.BeginTransaction();
                //    //db.Delete(product.Title, _selectedNomenclatureSubgroup);
                //    db.Transaction.Commit();
                //    MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK,
                //        MessageBoxImage.Information);
                //    //ShowFunctionAsync(3);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                //        MessageBoxImage.Error);
                //    db.Transaction.Rollback();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MiAddNomenclatureSubgroup_Click(object sender, RoutedEventArgs e)
        {
            //AddNomenclatureSubgroup add = new AddNomenclatureSubgroup(this);
            //add.ShowDialog();
            //ShowFunctionAsync(0);
        }

        #region Блокировка перемещения окна

        private void Window1_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source?.AddHook(WndProc);
        }

        private const int WmSyscommand = 0x0112;
        private const int ScMove = 0xF010;

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WmSyscommand:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == ScMove)
                    {
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }


        #endregion
    }
}

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public int IdStore, IdRole;
        //private int _selectedNomenclatureGroup = -1;
        private int _selectedNomenclatureSubgroup = -1;
        private readonly ViewModel _viewModel;

        public enum GroupForm : byte
        {
            ShowProduct,
            CashierWorkplace,
            ShowNomenclatureSubgroup
        }

        public MainWindow()
        {
            InitializeComponent();
            Form1 f = new Form1(this);
            f.ShowDialog();
            ConnectionString =
                $"Data Source={NameServer};Initial Catalog=Rul_base;Integrated Security=true";
            Authorization au = new Authorization(this);
            au.ShowDialog();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SourceInitialized += Window1_SourceInitialized;
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
            _viewModel = new ViewModel(Dispatcher.CurrentDispatcher);
            DataContext = _viewModel;
        }


        //Скрытие не нужных объектов с формы
        public void ShowGroup(GroupForm gf)
        {
            switch (gf)
            {
                case GroupForm.ShowProduct:
                    ProductGroup.Visibility = Visibility;
                    ProductGroup.Style = (Style) window.FindResource("ShowProductStackPanelStyle");
                    break;
                case GroupForm.CashierWorkplace:
                    ProductGroup.Visibility = Visibility.Hidden;
                    break;
                case GroupForm.ShowNomenclatureSubgroup:
                    ProductGroup.Visibility = Visibility.Hidden;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gf), gf, null);
            }
        }

        private async void ShowFunctionAsync(int choise) //choise: 0 - все; 1 - магазин; 2 - номенклатура; 3 - номенклатурная группа
        {
            //DgProducts.Items.Clear();
            switch (choise)
            {
                case 0:
                    await Task.Run(() => { ShowStoreFunction(); });
                    break;
                case 1:
                    await Task.Run(() => { ShowNomenclatureGroupFunction(); });
                    break;
                case 2:
                    //await Task.Run(() => { ShowNomenclatureSubgroupFunction(); });
                    break;
                case 3:
                   // await Task.Run(() => { ShowProductFunction(); });
                    break;
            }
        }

        private void ShowStoreFunction()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                Dispatcher.BeginInvoke((Action) (() => CbStore.Items.Clear()));
                string nameComm = $"select Store.title from Store where Store.ID = {IdStore}";
                SqlCommand command = new SqlCommand(nameComm, connection);
                string title = (string) command.ExecuteScalar();
                if (title != "")
                {
                    Dispatcher.BeginInvoke((Action) (() => CbStore.Items.Add(title)));
                    Dispatcher.BeginInvoke((Action) (() => CbStore.SelectedIndex = 0));
                }
            }
        }

        private void ShowNomenclatureGroupFunction()
        {
            bool check = false;
            Dispatcher.BeginInvoke((Action)(() => _viewModel.NomenclatureGroups.Clear()));
            NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(ConnectionString);
            var nomenclature = db.GetNomenclatureGroup(IdStore);
            foreach (var nom in nomenclature)
            {
                Dispatcher.BeginInvoke((Action)(() => _viewModel.NomenclatureGroups.Add(nom)));
                check = true;
            }
            if(check)
                Dispatcher.BeginInvoke((Action)(() => _viewModel.SelectedNomenclatureGroup = _viewModel.NomenclatureGroups[0]));
        }

        private void CbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbStore.SelectedIndex == -1) return;
            ShowFunctionAsync(1);
        }

        private void CbNomenclatureGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbNomenclatureGroup.SelectedIndex == -1) return;
           // ShowNomenclatureSubgroupFunction();
            //NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(ConnectionString);
            //db.FindNomenclatureGroupId(CbNomenclatureGroup.Items[CbNomenclatureGroup.SelectedIndex].ToString(),
            //    IdStore, ref _selectedNomenclatureGroup);
            //ShowFunctionAsync(2);
        }

        private void CbNomenclatureSubgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbNomenclatureSubgroup.SelectedIndex == -1) return;
            //NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(ConnectionString);
            //db.FindNomenclatureSubgroupId(
            //    CbNomenclatureSubgroup.Items[CbNomenclatureSubgroup.SelectedIndex].ToString(),
            //    _viewModel.SelectedNomenclatureGroup.Id, ref _selectedNomenclatureSubgroup);
            //ShowFunctionAsync(3);
        }

        private void MICashierWorkplace_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.CashierWorkplace);
        }

        private void MIShowProduct_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.ShowProduct);
            ShowFunctionAsync(0);
        }

        private void MiAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct ad = new AddProduct(this);
            ad.ShowDialog();
            ShowFunctionAsync(3);
        }

        private void MIChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            Product product = (Product) DgProducts.SelectedItem;
            ProductDataContext db = new ProductDataContext(ConnectionString);
            db.FindProductId(product.Title, _selectedNomenclatureSubgroup, ref id);
            AddProduct ad = new AddProduct(this, id);
            ad.ShowDialog();
            ShowFunctionAsync(3);
        }

        private void MiDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить товар?", "Удаление", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                int id = 0;
                Product product = (Product) DgProducts.SelectedItem;
                ProductDataContext db = new ProductDataContext(ConnectionString);
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.Delete(product.Title, _selectedNomenclatureSubgroup);
                    db.Transaction.Commit();
                    MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    ShowFunctionAsync(3);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    db.Transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MiAddNomenclatureSubgroup_Click(object sender, RoutedEventArgs e)
        {
            AddNomenclatureSubgroup add = new AddNomenclatureSubgroup(this);
            add.ShowDialog();
            ShowFunctionAsync(0);
        }

        private void MiShowNomenclatureSubgroup_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.ShowNomenclatureSubgroup);
            //TvShowGroup.Items.Add();
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

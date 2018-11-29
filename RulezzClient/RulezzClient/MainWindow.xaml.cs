using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
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
        public string ConnectionString =
            @"Data Source=DESKTOP-L7JMEC9;Initial Catalog=Rul_base;Integrated Security=false;User Id=God;Password=Dog";

        public int IdStore, IdRole;
        private readonly SqlConnection _connection;
        private int _selectedNomenclature = -1;
        private int _selectedNomenclatureGroup = -1;

        public enum GroupForm : byte { ShowProduct, CashierWorkplace, AddProduct}

        public MainWindow()
        {
            InitializeComponent();
            Authorization au = new Authorization(this);
            au.ShowDialog();
            _connection = new SqlConnection(ConnectionString);
            SourceInitialized += Window1_SourceInitialized;
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        //Скрытие не нужных объектов с формы
        public void ShowGroup(GroupForm gf)
        {
            switch (gf)
            {
                case GroupForm.ShowProduct:
                    ProductGroup.Visibility = Visibility;
                    ProductGroup.Style = (Style)window.FindResource("ShowProductStackPanelStyle");
                    break;
                case GroupForm.CashierWorkplace:
                    ProductGroup.Visibility = Visibility.Hidden;
                    break;
                case GroupForm.AddProduct:
                    ProductGroup.Visibility = Visibility.Visible;
                    ProductGroup.Style = (Style)window.FindResource("AddProductStackPanelStyle");
                    break;
            }
        }

        private void MIShowProduct_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.ShowProduct);
            ShowFunctionAsync(0);
        }

        private async void ShowFunctionAsync(int choise) //choise: 0 - все; 1 - магазин; 2 - номенклатура; 3 - номенклатурная группа
        {
            ProductListView.Items.Clear();
            switch (choise)
            {
                case 0:
                    StoreComboBox.Items.Clear();
                    await ShowStoreFunction();
                    break;
                case 1:
                    NomenclatureComboBox.Items.Clear();
                    await ShowNomenclatureFunction();
                    break;
                case 2:
                    NomenclatureGroupComboBox.Items.Clear();
                    await ShowNomenclatureGroupFunction();
                    break;
                case 3:
                    await ShowProductFunction();
                    break;
            }
        }

        private Task ShowStoreFunction()
        {
            Task t = new Task(() =>
            {
                string nameComm = $"select Store.title from Store where Store.ID = {IdStore}";
                SqlCommand command = new SqlCommand(nameComm, _connection);
                string title = (string)command.ExecuteScalar();
                if (title != "")
                {
                    Dispatcher.BeginInvoke((Action)(() => StoreComboBox.Items.Add(title)));
                    Dispatcher.BeginInvoke((Action)(() => StoreComboBox.SelectedIndex = 0));
                }
            });
            t.Start();
            return t;
        }

        private Task ShowNomenclatureFunction()
        {
            Task t = new Task(() =>
            {
                NomenclatureDataContext db = new NomenclatureDataContext(ConnectionString);
                foreach (var nom in db.GetNomenclature(IdStore))
                {
                    Dispatcher.BeginInvoke((Action)(() => NomenclatureComboBox.Items.Add(nom.Title)));
                    Dispatcher.BeginInvoke((Action)(() => NomenclatureComboBox.SelectedIndex = 0));
                }
            });
            t.Start();
            return t;
        }

        private Task ShowNomenclatureGroupFunction()
        {
            Task t = new Task(() =>
            {
                NomenclatureGroupeDataContext db = new NomenclatureGroupeDataContext(ConnectionString);
                foreach (var nom in db.GetNomenclatureGroup(_selectedNomenclature))
                {
                    Dispatcher.BeginInvoke((Action)(() => NomenclatureGroupComboBox.Items.Add(nom.Title)));
                    Dispatcher.BeginInvoke((Action)(() => NomenclatureGroupComboBox.SelectedIndex = 0));
                }
            });
            t.Start();
            return t;
        }

        private Task ShowProductFunction()
        {
            Task t = new Task(() =>
            {
                ProductDataContext db = new ProductDataContext(ConnectionString);
                foreach (var pro in db.GetProduct(_selectedNomenclatureGroup))
                {
                    Dispatcher.BeginInvoke((Action)(() => ProductListView.Items.Add(pro)));
                }
            });
            t.Start();
            return t;
        }

        private void Store_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //NomenclatureDataContext db = new NomenclatureDataContext(ConnectionString);
            //db.FindNomenclatureId(NomenclatureComboBox.Items[NomenclatureComboBox.SelectedIndex].ToString(), IdStore, ref _selectedNomenclature);
            ShowFunctionAsync(1);
        }

        private void Nomenclature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NomenclatureComboBox.SelectedIndex == -1) return;
            NomenclatureDataContext db = new NomenclatureDataContext(ConnectionString);
            db.FindNomenclatureId(NomenclatureComboBox.Items[NomenclatureComboBox.SelectedIndex].ToString(),
                IdStore, ref _selectedNomenclature);
            ShowFunctionAsync(2);
        }

        private void NomenclatureGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NomenclatureGroupComboBox.SelectedIndex == -1) return;
            NomenclatureGroupeDataContext db = new NomenclatureGroupeDataContext(ConnectionString);
            db.FindNomenclatureGroupId(
                NomenclatureGroupComboBox.Items[NomenclatureGroupComboBox.SelectedIndex].ToString(),
                _selectedNomenclature, ref _selectedNomenclatureGroup);
            ShowFunctionAsync(3);
        }
        
        private void MICashierWorkplace_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.CashierWorkplace);
        }

        private void MiAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ShowGroup(GroupForm.AddProduct);
        }

        //Блокировка перемещения окна
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

    }
}

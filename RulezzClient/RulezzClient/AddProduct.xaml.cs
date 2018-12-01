using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RulezzClient
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly MainWindow _mw;
        private int _selectedNomenclature;
        private int _selectedNomenclatureGroup;
        private int _selectedUnitSt;
        private int _selectedWarranty;

        public AddProduct(MainWindow mw)
        {
            this.Title = "Добавить товар";
            InitializeComponent();
            _mw = mw;
            ReadWuaranteePeriodAsync().GetAwaiter();
            ReadUnitStorageAsync().GetAwaiter();
            ShowFunctionAsync(1);
        }

        public AddProduct(MainWindow mw, int id_prod)
        {
            InitializeComponent();
            this.Title = "Изменить товар";
            _mw = mw;
            ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
            db.GetListProduct(-1, id_prod);
            ProductView product = db.GetListProduct(-1, id_prod).First();
            ReadWuaranteePeriodAsync().GetAwaiter();
            ReadUnitStorageAsync().GetAwaiter();
            ShowFunctionAsync(1);
        }

        private async Task ReadUnitStorageAsync()
        {
            SqlConnection connection = new SqlConnection(_mw.ConnectionString);
            connection.Open();
            const string nameComm = "select title from UnitStorage";
            SqlCommand command = new SqlCommand(nameComm, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    CbUnitSt.Items.Add(reader.GetValue(0));
                }
            }
            reader.Close();
            if (CbUnitSt.Items.Count != 0)
            {
                CbUnitSt.SelectedIndex = 0;
            }
            command.Cancel();
            connection.Close();
        }

        private async Task ReadWuaranteePeriodAsync()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_mw.ConnectionString);
                connection.Open();
                const string nameComm = "select * from FunViewWarrantyPeriod()";
                SqlCommand command = new SqlCommand(nameComm, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        CbWarranty.Items.Add(reader.GetValue(0));
                    }
                }
                reader.Close();
                connection.Close();
                //await Task.Run(() =>
                //{
                //    WarrantyPeriodDataContext db = new WarrantyPeriodDataContext(_mw.ConnectionString);
                //    try
                //    {
                //        foreach (var war in db.GetListWarrantyPeriod())
                //        {
                //            Dispatcher.BeginInvoke((Action) (() => CbWarranty.Items.Add(war)));
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                //            MessageBoxImage.Error);
                //    }
                //});
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void ShowFunctionAsync(int choise) //choise: 1 - номенклатура; 2 - номенклатурyная группа;
        {
            switch (choise)
            {
                case 1:
                    await Task.Run(() => { ShowNomenclatureFunction(); });
                    break;
                case 2:
                    await Task.Run(() => { ShowNomenclatureGroupFunction(); });
                    break;
            }
        }

        private void ShowNomenclatureFunction()
        {
            Dispatcher.BeginInvoke((Action)(() => CbNomenclature.Items.Clear()));
            NomenclatureDataContext db = new NomenclatureDataContext(_mw.ConnectionString);
            var nomenclature = db.GetNomenclature(_mw.IdStore);
            foreach (var nom in nomenclature)
            {
                Dispatcher.BeginInvoke((Action)(() => CbNomenclature.Items.Add(nom.Title)));
            }
            Dispatcher.BeginInvoke((Action)(() => CbNomenclature.SelectedIndex = 0));
        }

        private void ShowNomenclatureGroupFunction()
        {
            Dispatcher.BeginInvoke((Action)(() => NomenclatureGroupComboBox.Items.Clear()));
            NomenclatureGroupeDataContext db = new NomenclatureGroupeDataContext(_mw.ConnectionString);
            foreach (var nom in db.GetNomenclatureGroup(_selectedNomenclature))
            {
                Dispatcher.BeginInvoke((Action)(() => NomenclatureGroupComboBox.Items.Add(nom.Title)));
            }
            Dispatcher.BeginInvoke((Action)(() => NomenclatureGroupComboBox.SelectedIndex = 0));
        }

        private void Nomenclature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbNomenclature.SelectedIndex == -1) return;
            NomenclatureDataContext db = new NomenclatureDataContext(_mw.ConnectionString);
            db.FindNomenclatureId(CbNomenclature.Items[CbNomenclature.SelectedIndex].ToString(),
                _mw.IdStore, ref _selectedNomenclature);
            ShowFunctionAsync(2);
        }

        private void NomenclatureGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NomenclatureGroupComboBox.SelectedIndex == -1) return;
            NomenclatureGroupeDataContext db = new NomenclatureGroupeDataContext(_mw.ConnectionString);
            db.FindNomenclatureGroupId(
                NomenclatureGroupComboBox.Items[NomenclatureGroupComboBox.SelectedIndex].ToString(),
                _selectedNomenclature, ref _selectedNomenclatureGroup);
        }

        private async Task GetIdUnitStorageAsync()
        {
            using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
            {
                connection.Open();
                string nameComm =
                    $"select ID from UnitStorage where title = '{CbUnitSt.Items[CbUnitSt.SelectedIndex]}'";
                SqlCommand command = new SqlCommand(nameComm, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        _selectedUnitSt = (int) reader.GetValue(0);
                        reader.Close();
                        return;
                    }
                }

                reader.Close();
            }
        }

        private void CbUnitSt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbUnitSt.SelectedIndex == -1) return;
            GetIdUnitStorageAsync().GetAwaiter();
        }

        private async Task GetIdWuaranteePeriodAsync()
        {
            using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
            {
                connection.Open();
                string nameComm =
                    $"select ID from WuaranteePeriod where period = {CbWarranty.Items[CbWarranty.SelectedIndex]}";
                SqlCommand command = new SqlCommand(nameComm, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        _selectedWarranty = (int) reader.GetValue(0);
                        reader.Close();
                        return;
                    }
                }

                reader.Close();
            }
        }

        private void CbWarranty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbWarranty.SelectedIndex == -1) return;
            GetIdWuaranteePeriodAsync().GetAwaiter();
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    command.CommandText =
                        "INSERT INTO Product (id_nomenclature_group, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, warranty, sales_price)" +
                        $" VALUES({_selectedNomenclatureGroup}, '{TbName.Text}', '{TbItemNum.Text}', '{TbBarcode.Text}', {_selectedUnitSt}, 0, {TbPurchasePrice.Text}, 1, 1, {TbSalesPrice.Text})";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    transaction.Rollback();
                }
            }
        }
    }
}

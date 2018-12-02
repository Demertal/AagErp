using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
        private int _selectedNomenclatureGroup;
        private int _selectedNomenclatureSubgroup;
        private int _selectedUnitSt;
        private int _selectedWarranty;
        private int _selectedExchangeRates;
        private ProductView _product;

        public AddProduct(MainWindow mw)
        {
            this.Title = "Добавить товар";
            InitializeComponent();
            BAdd.Content = "Добавить";
            _mw = mw;
            ReadWuaranteePeriodAsync().GetAwaiter();
            ReadUnitStorageAsync().GetAwaiter();
            ReadExchangeRatesAsync().GetAwaiter();
            ShowFunctionAsync(1);
        }

        public AddProduct(MainWindow mw, int id_prod)
        {
            InitializeComponent();
            this.Title = "Изменить товар";
            BAdd.Content = "Изменить";
            _mw = mw;
            try
            {
                ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
                db.GetListProduct(-1, id_prod);
                _product = db.GetListProduct(-1, id_prod).First();
                ReadWuaranteePeriodAsync().GetAwaiter();
                ReadUnitStorageAsync().GetAwaiter();
                ReadExchangeRatesAsync().GetAwaiter();
                ShowFunctionAsync(1);
                TbName.Text = _product.Title;
                TbItemNum.Text = _product.VendorCode;
                TbBarcode.Text = _product.Barcode;
                TbPurchasePrice.Text = _product.PurchasePrice.ToString(CultureInfo.InvariantCulture);
                TbSalesPrice.Text = _product.SalesPrice.ToString(CultureInfo.InvariantCulture);
                TbName.Text = _product.Title;
               
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task ReadUnitStorageAsync()
        {
            try
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
                if (_product != null)
                {
                    for (int i = 0; i < CbUnitSt.Items.Count; i++)
                    {
                        if ((string)CbUnitSt.Items[i] == _product.UnitStorage)
                            CbWarranty.SelectedIndex = i;
                    }
                }

                command.Cancel();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task ReadWuaranteePeriodAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    WarrantyPeriodDataContext db = new WarrantyPeriodDataContext(_mw.ConnectionString);
                    try
                    {
                        foreach (var war in db.GetListWarrantyPeriod(-1))
                        {
                            Dispatcher.BeginInvoke((Action)(() => CbWarranty.Items.Add(war.Period)));
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                });
                if (CbWarranty.Items.Count != 0)
                {
                    CbWarranty.SelectedIndex = 0;
                }

                if (_product != null)
                {
                    for (int i = 0; i < CbWarranty.Items.Count; i++)
                    {
                        if ((string) CbWarranty.Items[i] == _product.Warranty)
                            CbWarranty.SelectedIndex = i;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task ReadExchangeRatesAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    ExchangeRatesDataContext db = new ExchangeRatesDataContext(_mw.ConnectionString);
                    try
                    {
                        foreach (var ex in db.GetListExchangeRates())
                        {
                            Dispatcher.BeginInvoke((Action)(() => CbExRates.Items.Add(ex.Currency)));
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                });
                if (CbExRates.Items.Count != 0)
                {
                    CbExRates.SelectedIndex = 0;
                }
                if (_product != null)
                {
                    for (int i = 0; i < CbExRates.Items.Count; i++)
                    {
                        if ((string)CbExRates.Items[i] == _product.ExchangeRates)
                            CbExRates.SelectedIndex = i;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void ShowFunctionAsync(int choise) //choise: 1 - номенклатура; 2 - номенклатурyная группа;
        {
            try
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ShowNomenclatureFunction()
        {
            try
            {
                Dispatcher.BeginInvoke((Action) (() => CbNomenclatureGroup.Items.Clear()));
                NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_mw.ConnectionString);
                var nomenclature = db.GetNomenclatureGroup(_mw.IdStore);
                foreach (var nom in nomenclature)
                {
                    Dispatcher.BeginInvoke((Action) (() => CbNomenclatureGroup.Items.Add(nom.Title)));
                }

                Dispatcher.BeginInvoke((Action) (() => CbNomenclatureGroup.SelectedIndex = 0));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ShowNomenclatureGroupFunction()
        {
            try
            {
                Dispatcher.BeginInvoke((Action) (() => CbNomenclatureSubgroup.Items.Clear()));
                NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(_mw.ConnectionString);
                foreach (var nom in db.GetNomenclatureSubgroup(_selectedNomenclatureGroup))
                {
                    Dispatcher.BeginInvoke((Action) (() => CbNomenclatureSubgroup.Items.Add(nom.Title)));
                }

                Dispatcher.BeginInvoke((Action) (() => CbNomenclatureSubgroup.SelectedIndex = 0));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbNomenclatureGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbNomenclatureGroup.SelectedIndex == -1) return;
                NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_mw.ConnectionString);
                db.FindNomenclatureGroupId(CbNomenclatureGroup.Items[CbNomenclatureGroup.SelectedIndex].ToString(),
                    _mw.IdStore, ref _selectedNomenclatureGroup);
                ShowFunctionAsync(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbNomenclatureSubgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbNomenclatureSubgroup.SelectedIndex == -1) return;
                NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(_mw.ConnectionString);
                db.FindNomenclatureSubgroupId(
                    CbNomenclatureSubgroup.Items[CbNomenclatureSubgroup.SelectedIndex].ToString(),
                    _selectedNomenclatureGroup, ref _selectedNomenclatureSubgroup);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task GetIdUnitStorageAsync()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbUnitSt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbUnitSt.SelectedIndex == -1) return;
                GetIdUnitStorageAsync().GetAwaiter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task GetIdWarrantyAsync(string warranty)
        {
            try
            {
                await Task.Run(() =>
                {
                    WarrantyPeriodDataContext db = new WarrantyPeriodDataContext(_mw.ConnectionString);
                    db.GetWarrantyPeriodId(warranty, ref _selectedWarranty);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbWarranty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbWarranty.SelectedIndex == -1) return;
                GetIdWarrantyAsync(CbWarranty.Items[CbWarranty.SelectedIndex].ToString()).GetAwaiter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    try
                    {
                        if (_product == null)
                        {
                            command.CommandText =
                                "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
                                $" VALUES({_selectedNomenclatureSubgroup}, '{TbName.Text}', '{TbItemNum.Text}', '{TbBarcode.Text}', {_selectedUnitSt}, 0, {TbPurchasePrice.Text}, {_selectedExchangeRates}, {_selectedWarranty}, {TbSalesPrice.Text})";
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            int id = 0;
                            ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
                            db.FindProductId(_product.Barcode, _selectedNomenclatureSubgroup, ref id);
                            command.CommandText =
                                $"UPDATE Product Set id_nomenclature_subgroup = {_selectedNomenclatureSubgroup}, title = '{TbName.Text}', vendor_code = '{TbItemNum.Text}', barcode = '{TbBarcode.Text}', id_unit_storage = {_selectedUnitSt}, purchase_price = {TbPurchasePrice.Text}, id_exchange_rates = {_selectedExchangeRates}, id_warranty = {_selectedWarranty}, sales_price = {TbSalesPrice.Text} where id = {id}";
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            MessageBox.Show("Товар изменен.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbExRates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbExRates.SelectedIndex == -1) return;
                ExchangeRatesDataContext db = new ExchangeRatesDataContext(_mw.ConnectionString);
                db.FindExchangeRatesId(
                    CbExRates.Items[CbExRates.SelectedIndex].ToString(),
                    ref _selectedExchangeRates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

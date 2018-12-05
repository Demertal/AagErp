using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RulezzClient
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct
    {
        private readonly MainWindow _mw;
        private int _selectedNomenclatureGroup;
        private int _selectedNomenclatureSubgroup;
        private int _selectedUnitSt;
        private int _selectedWarranty;
        private readonly Product _product;

        public AddProduct(MainWindow mw)
        {
            Title = "Добавить товар";
            InitializeComponent();
            BAdd.Content = "Добавить";
            _mw = mw;
            ReadWuaranteePeriodAsync().GetAwaiter();
            ReadUnitStorageAsync().GetAwaiter();
            ShowFunctionAsync(1);
        }

        public AddProduct(MainWindow mw, int idProd)
        {
            InitializeComponent();
            Title = "Изменить товар";
            BAdd.Content = "Изменить";
            _mw = mw;
            try
            {
                ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
                db.GetListProduct(-1, idProd);
                _product = db.GetListProduct(-1, idProd).First();
                ReadWuaranteePeriodAsync().GetAwaiter();
                ReadUnitStorageAsync().GetAwaiter();
                ShowFunctionAsync(1);
                TbName.Text = _product.Title;
                TbItemNum.Text = _product.VendorCode;
                TbBarcode.Text = _product.Barcode;
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

        private async void ShowFunctionAsync(int choise) //choise: 1 - номенклатура; 2 - номенклатурyная группа;
        {
            try
            {
                switch (choise)
                {
                    case 1:
                        await Task.Run(() => { ShowNomenclatureGroupFunction(); });
                        break;
                    case 2:
                        await Task.Run(() => { ShowNomenclatureSubgroupFunction(); });
                        break;
                }
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

        private void ShowNomenclatureSubgroupFunction()
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
                //if (CbNomenclatureGroup.SelectedIndex == -1) return;
                //NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_mw.ConnectionString);
                //db.FindNomenclatureGroupId(CbNomenclatureGroup.Items[CbNomenclatureGroup.SelectedIndex].ToString(),
                //    _mw.IdStore, ref _selectedNomenclatureGroup);
                //ShowFunctionAsync(2);
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
                //NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(_mw.ConnectionString);
                //db.FindNomenclatureSubgroupId(
                //    CbNomenclatureSubgroup.Items[CbNomenclatureSubgroup.SelectedIndex].ToString(),
                //    _selectedNomenclatureGroup, ref _selectedNomenclatureSubgroup);
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
                            int id = 0;
                            ExchangeRatesDataContext db = new ExchangeRatesDataContext(_mw.ConnectionString);
                            db.FindExchangeRatesId("грн", ref id);

                            command.CommandText =
                                "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
                                $" VALUES({_selectedNomenclatureSubgroup}, '{TbName.Text}', '{TbItemNum.Text}', '{TbBarcode.Text}', {_selectedUnitSt}, 0, 0, {id}, {_selectedWarranty}, 0)";
                            if (command.ExecuteNonQuery() == 1)
                            {
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("При попытке добавить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            int id = 0;
                            ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
                            db.FindProductId(_product.Barcode, _selectedNomenclatureSubgroup, ref id);
                            command.CommandText =
                                $"UPDATE Product Set id_nomenclature_subgroup = {_selectedNomenclatureSubgroup}, title = '{TbName.Text}', vendor_code = '{TbItemNum.Text}', barcode = '{TbBarcode.Text}', id_unit_storage = {_selectedUnitSt}, id_warranty = {_selectedWarranty} where id = {id}";

                            if (command.ExecuteNonQuery() == 1)
                            {
                                transaction.Commit();
                                MessageBox.Show("Товар изменен.", "Успех", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("При попытке изенить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                transaction.Rollback();
                            }
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
    }
}

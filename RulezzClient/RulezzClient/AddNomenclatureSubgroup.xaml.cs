using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RulezzClient
{
    /// <summary>
    /// Логика взаимодействия для AddNomenclatureSubgroup.xaml
    /// </summary>
    public partial class AddNomenclatureSubgroup
    {
        private readonly MainWindow _mw;
        private int _selectedNomenclatureGroup;

        public AddNomenclatureSubgroup(MainWindow mw)
        {
            _mw = mw;
            InitializeComponent();
            Task.Run(() => ShowNomenclatureGroupFunction()).GetAwaiter();
        }

        private void ShowNomenclatureGroupFunction()
        {
            try
            {
                Dispatcher.BeginInvoke((Action)(() => CbNomenclatureGroupNew.Items.Clear()));
                NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_mw.ConnectionString);
                var nomenclature = db.GetNomenclatureGroup(_mw.IdStore);
                foreach (var nom in nomenclature)
                {
                    Dispatcher.BeginInvoke((Action)(() => CbNomenclatureGroupNew.Items.Add(nom.Title)));
                }

                Dispatcher.BeginInvoke((Action)(() => CbNomenclatureGroupNew.SelectedIndex = 0));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CbNomenclatureGroupNew_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (CbNomenclatureGroupNew.SelectedIndex == -1) return;
                //NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_mw.ConnectionString);
                //db.FindNomenclatureGroupId(CbNomenclatureGroupNew.Items[CbNomenclatureGroupNew.SelectedIndex].ToString(),
                //    _mw.IdStore, ref _selectedNomenclatureGroup);
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
                        command.CommandText =
                            "INSERT INTO NomenclatureSubgroup (title, id_nomenclature_group, id_price_grop)" +
                            $" VALUES('{TbNameNew.Text}', '{_selectedNomenclatureGroup}', 2)";
                        if (command.ExecuteNonQuery() == 1)
                        {
                            transaction.Commit();
                            MessageBox.Show("Номенклатурная подгруппа добавлена.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("При попытке добавить номенклатурную подгруппу произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            transaction.Rollback();
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

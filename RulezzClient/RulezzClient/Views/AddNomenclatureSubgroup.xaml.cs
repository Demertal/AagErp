using System;
using System.Data.SqlClient;
using System.Windows;

namespace RulezzClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AddNomenclatureSubgroup.xaml
    /// </summary>
    public partial class AddNomenclatureSubgroup
    {
        //private readonly MainWindow _mw;

        public AddNomenclatureSubgroup()
        {
            InitializeComponent();
        }

    //    private void BAdd_Click(object sender, RoutedEventArgs e)
    //    {
    //        try
    //        {
    //            using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
    //            {
    //                connection.Open();
    //                SqlTransaction transaction = connection.BeginTransaction();

    //                SqlCommand command = connection.CreateCommand();
    //                command.Transaction = transaction;

    //                try
    //                {
    //                    //command.CommandText =
    //                    //    "INSERT INTO NomenclatureSubgroup (title, id_nomenclature_group, id_price_grop)" +
    //                    //    $" VALUES('{TbNameNew.Text}', '{_selectedNomenclatureGroup}', 2)";
    //                    if (command.ExecuteNonQuery() == 1)
    //                    {
    //                        transaction.Commit();
    //                        MessageBox.Show("Номенклатурная подгруппа добавлена.", "Успех", MessageBoxButton.OK,
    //                            MessageBoxImage.Information);
    //                    }
    //                    else
    //                    {
    //                        MessageBox.Show("При попытке добавить номенклатурную подгруппу произошла ошибка.", "Ошибка", MessageBoxButton.OK,
    //                            MessageBoxImage.Error);
    //                        transaction.Rollback();
    //                    }
    //                    Close();
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
    //                        MessageBoxImage.Error);
    //                    transaction.Rollback();
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
    //                MessageBoxImage.Error);
    //        }
    //    }
    }
}

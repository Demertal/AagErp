using System.Windows.Controls;

namespace RulezzClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : UserControl
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        //private void BAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    //    using (SqlConnection connection = new SqlConnection(_mw.ConnectionString))
        //    //    {
        //    //        connection.Open();
        //    //        SqlTransaction transaction = connection.BeginTransaction();

        //    //        SqlCommand command = connection.CreateCommand();
        //    //        command.Transaction = transaction;

        //    //        try
        //    //        {
        //    //            if (_product == null)
        //    //            {
        //    //                int id = 0;
        //    //                ExchangeRatesDataContext db = new ExchangeRatesDataContext(_mw.ConnectionString);
        //    //                db.FindExchangeRatesId("грн", ref id);

        //    //                //command.CommandText =
        //    //                //    "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
        //    //                //    $" VALUES({_selectedNomenclatureSubgroup}, '{TbName.Text}', '{TbItemNum.Text}', '{TbBarcode.Text}', {_selectedUnitSt}, 0, 0, {id}, {_selectedWarranty}, 0)";
        //    //                if (command.ExecuteNonQuery() == 1)
        //    //                {
        //    //                    command.ExecuteNonQuery();
        //    //                    transaction.Commit();
        //    //                    MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
        //    //                        MessageBoxImage.Information);
        //    //                }
        //    //                else
        //    //                {
        //    //                    MessageBox.Show("При попытке добавить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
        //    //                        MessageBoxImage.Error);
        //    //                    transaction.Rollback();
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                int id = 0;
        //    //                ProductDataContext db = new ProductDataContext(_mw.ConnectionString);
        //    //                //db.FindProductId(_product.Barcode, _selectedNomenclatureSubgroup, ref id);
        //    //                //command.CommandText =
        //    //                //    $"UPDATE Product Set id_nomenclature_subgroup = {_selectedNomenclatureSubgroup}, title = '{TbName.Text}', vendor_code = '{TbItemNum.Text}', barcode = '{TbBarcode.Text}', id_unit_storage = {_selectedUnitSt}, id_warranty = {_selectedWarranty} where id = {id}";

        //    //                if (command.ExecuteNonQuery() == 1)
        //    //                {
        //    //                    transaction.Commit();
        //    //                    MessageBox.Show("Товар изменен.", "Успех", MessageBoxButton.OK,
        //    //                        MessageBoxImage.Information);
        //    //                }
        //    //                else
        //    //                {
        //    //                    MessageBox.Show("При попытке изенить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
        //    //                        MessageBoxImage.Error);
        //    //                    transaction.Rollback();
        //    //                }
        //    //            }
        //    //            Close();
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
        //    //                MessageBoxImage.Error);
        //    //            transaction.Rollback();
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
        //    //        MessageBoxImage.Error);
        //    //}
        //}
    }
}

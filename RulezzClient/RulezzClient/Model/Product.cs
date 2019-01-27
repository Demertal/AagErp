using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace RulezzClient.Model
{
   public class Product : BindableBase, IObject
   {
        private int _id;
        private string _title;
        private string _vendorCode;
        private string _barcode;
        private int _count;
        private string _unitStorage;
        private decimal _salesPrice;
        private decimal _purchasePrice;
        private string _warranty;
        private string _exchangeRates;
        private int _idNomenclatureSubgroup;

        public Product()
        {
            Title = "";
            Id = -1;
            VendorCode = "";
            Barcode = "";
            Count = -1;
            UnitStorage = "";
            SalesPrice = -1;
            PurchasePrice = -1;
            Warranty = "";
            ExchangeRates = "";
            IdNomenclatureSubgroup = -1;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "vendor_code")]
        public string VendorCode
        {
            get => _vendorCode;
            set
            {
                _vendorCode = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "barcode")]
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "count")]
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "unit_storage")]
        public string UnitStorage
        {
            get => _unitStorage;
            set
            {
                _unitStorage = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "sales_price")]
        public decimal SalesPrice
        {
            get => _salesPrice;
            set
            {
                _salesPrice = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "purchase_price")]
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "warranty")]
        public string Warranty
        {
            get => _warranty;
            set
            {
                _warranty = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "exchange_rates")]
        public string ExchangeRates
        {
            get => _exchangeRates;
            set
            {
                _exchangeRates = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "id_nomenclature_subgroup")]
        public int IdNomenclatureSubgroup
        {
            get => _idNomenclatureSubgroup;
            set
            {
                _idNomenclatureSubgroup = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(Product other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title && VendorCode == other.VendorCode &&
                   Barcode == other.Barcode && Count == other.Count && UnitStorage == other.UnitStorage &&
                   SalesPrice == other.SalesPrice && PurchasePrice == other.PurchasePrice &&
                   Warranty == other.Warranty && ExchangeRates == other.ExchangeRates && IdNomenclatureSubgroup == other.IdNomenclatureSubgroup;
        }

        private static List<Product> Load(string connectionString, int idNomenclatureSubgroup)
        {
            List<Product> listProduct = new List<Product>();
            try
            {
                ProductDataContext dc = new ProductDataContext(connectionString);
                var product = dc.Load(idNomenclatureSubgroup);
                foreach (var pr in product)
                {
                    listProduct.Add(pr);
                }
                return listProduct;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<Product>> AsyncLoad(string connectionString, int idNomenclatureSubgroup)
        {
            return await Task.Run(() => Load(connectionString, idNomenclatureSubgroup));
        }

        public bool Add(string connectionString)
        {
            bool success;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    try
                    {
                        int id = 0;
                        ExchangeRatesDataContext db = new ExchangeRatesDataContext(connectionString);
                        db.FindExchangeRatesId("грн", ref id);

                        command.CommandText =
                            "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
                            $" VALUES({_idNomenclatureSubgroup}, '{_title}', '{_vendorCode}', '{_barcode}', {_unitStorage}, 0, 0, {id}, {_warranty}, 0)";
                        if (command.ExecuteNonQuery() == 1)
                        {
                            transaction.Commit();
                            MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            success = true;
                        }
                        else
                        {
                            MessageBox.Show("При попытке добавить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            transaction.Rollback();
                            success = false;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        transaction.Rollback();
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                success = false;
            }

            return success;
        }

        public bool Delete(string connectionString)
        {
            bool success;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = $"DELETE Product where Product.ID = {_id}";
                        if (command.ExecuteNonQuery() == 1)
                        {
                            transaction.Commit();
                            MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            success = true;
                        }
                        else
                        {
                            MessageBox.Show("При попытке удалить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            transaction.Rollback();
                            success = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        transaction.Rollback();
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                success = false;
            }

            return success;
        }
    }

    public class ProductDataContext : DataContext
    {
        public ProductDataContext(string connectionString)
            : base(connectionString)
        {
        }

        //Получение продукта по id номенклатурной подгруппы
        [Function(Name = "FunViewProduct", IsComposable = true)]
        public IQueryable<Product> Load(
            [Parameter(Name = "id_nomenclature_subgroup", DbType = "int")]
            int idNomSub)
        {
            return CreateMethodCallQuery<Product>(this, (MethodInfo) MethodBase.GetCurrentMethod(), idNomSub);
        }
    }
}

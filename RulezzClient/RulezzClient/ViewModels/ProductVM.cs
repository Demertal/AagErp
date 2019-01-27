using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ProductListVm : BindableBase
    {
        private readonly ObservableCollection<Product> _products = new ObservableCollection<Product>();

        public ReadOnlyObservableCollection<Product> Products;

        public ProductListVm()
        {
            Products = new ReadOnlyObservableCollection<Product>(_products);
        }

        public async Task<List<Product>> Load(int idNomenclatureSubGroup)
        {
            List<Product> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.Product.SqlQuery("Select * From Product WHERE Product.IdNomenclatureSubGroup = @idNomenclatureSubGroup",
                        new SqlParameter("@idNomenclatureSubGroup", idNomenclatureSubGroup)).ToList();
                }
            });


            _products.Clear();
            foreach (var t in temp)
            {
                _products.Add(t);
            }

            return temp;
        }

        public void Delete(int id)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var product = db.Product.Find(id);
                        db.Entry(product).State = EntityState.Deleted;
                        db.SaveChanges();
                        transaction.Commit();
                        MessageBox.Show("Товар удален", "Успех", MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

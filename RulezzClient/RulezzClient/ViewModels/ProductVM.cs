using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ProductListVm : BindableBase
    {
        private readonly ObservableCollection<ProductView_Result> _products = new ObservableCollection<ProductView_Result>();

        public ReadOnlyObservableCollection<ProductView_Result> Products;

        public ProductListVm()
        {
            Products = new ReadOnlyObservableCollection<ProductView_Result>(_products);
        }

        public async Task<List<ProductView_Result>> Load(int idNomenclatureSubGroup)
        {
            List<ProductView_Result> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.ProductView(idNomenclatureSubGroup).ToList();
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

using System;
using System.Collections;
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
        private readonly ObservableCollection<ProductView> _products = new ObservableCollection<ProductView>();

        public ReadOnlyObservableCollection<ProductView> Products;

        public ProductListVm()
        {
            Products = new ReadOnlyObservableCollection<ProductView>(_products);
        }

        public async Task<List<ProductView>> LoadByNomenclatureSubGroup(int idNomenclatureSubGroup)
        {
            List<ProductView> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.ProductView(idNomenclatureSubGroup).Select(obj => new ProductView{
                        Id = obj.Id,
                        Barcode = obj.Barcode,
                        Count = obj.Count,
                        ExchangeRate = obj.ExchangeRate,
                        UnitStorage = obj.UnitStorage,
                        WarrantyPeriod = obj.WarrantyPeriod,
                        PurchasePrice = obj.PurchasePrice,
                        SalesPrice = obj.SalesPrice,
                        Title = obj.Title,
                        VendorCode = obj.VendorCode,
                        IdNomenclatureSubGroup = obj.IdNomenclatureSubGroup}).ToList();
                }
            });

            _products.Clear();
            foreach (var t in temp)
            {
                _products.Add(t);
            }

            return temp;
        }

        public async Task<List<ProductView>> LoadAll(int idStore)
        {
            List<ProductView> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.AllProductView(idStore).Select(obj => new ProductView
                    {
                        Id = obj.Id,
                        Barcode = obj.Barcode,
                        Count = obj.Count,
                        ExchangeRate = obj.ExchangeRate,
                        UnitStorage = obj.UnitStorage,
                        WarrantyPeriod = obj.WarrantyPeriod,
                        PurchasePrice = obj.PurchasePrice,
                        SalesPrice = obj.SalesPrice,
                        Title = obj.Title,
                        VendorCode = obj.VendorCode,
                        IdNomenclatureSubGroup = obj.IdNomenclatureSubGroup
                    }).ToList();
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

//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Threading;
//using Prism.Mvvm;
//using RulezzClient.Models;

//namespace RulezzClient.ViewModels
//{
//    class ProductListVm : BindableBase
//    {
//        private readonly ObservableCollection<ProductView> _products = new ObservableCollection<ProductView>();

//        public ReadOnlyObservableCollection<ProductView> Products;

//        public ProductListVm()
//        {
//            Products = new ReadOnlyObservableCollection<ProductView>(_products);
//        }

//        public async Task<int> Load(int idGroup)
//        {
//            List<ProductView> temp = await Task.Run(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    db.Products.Load();
//                    return db.Products.Where(obj => obj.IdGroup == idGroup).Select(obj => new ProductView
//                    {
//                        Id = obj.Id,
//                        Title = obj.Title,
//                        VendorCode = obj.VendorCode,
//                        Barcode = obj.Barcode,
//                        PurchasePrice = obj.PurchasePrice,
//                        SalesPrice = obj.SalesPrice,
//                        IdUnitStorage = obj.IdUnitStorage,
//                        IdExchangeRate = obj.IdExchangeRate,
//                        IdWarrantyPeriod = obj.IdWarrantyPeriod,
//                        IdGroup = obj.IdGroup,
//                        ExchangeRate = obj.ExchangeRates,
//                        UnitStorage = obj.UnitStorages,
//                        WarrantyPeriod = obj.WarrantyPeriods
//                    }).ToList();
//                }
//            });
//            _products.Clear();
//            if (temp != null)
//            {
//                foreach (var item in temp)
//                {
//                    _products.Add(item);
//                }

//                return _products.Count;
//            }
//            return 0;
//        }

//        public async Task<int> LoadByFindString(string findString)
//        {
//            List<ProductView> temp = await Task.Run(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    db.Products.Load();
//                    return db.Products.Where(obj => obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) || obj.Barcode.Contains(findString)).Select(obj => new ProductView
//                    {
//                        Id = obj.Id,
//                        Title = obj.Title,
//                        VendorCode = obj.VendorCode,
//                        Barcode = obj.Barcode,
//                        PurchasePrice = obj.PurchasePrice,
//                        SalesPrice = obj.SalesPrice,
//                        IdUnitStorage = obj.IdUnitStorage,
//                        IdExchangeRate = obj.IdExchangeRate,
//                        IdWarrantyPeriod = obj.IdWarrantyPeriod,
//                        ExchangeRate = obj.ExchangeRates,
//                        UnitStorage = obj.UnitStorages,
//                        WarrantyPeriod = obj.WarrantyPeriods,
//                    }).ToList();
//                }
//            });
//            _products.Clear();
//            if (temp != null)
//            {
//                foreach (var item in temp)
//                {
//                    _products.Add(item);
//                }

//                return _products.Count;
//            }
//            return 0;
//        }

//        public void Delete(int id)
//        {
//            using (StoreEntities db = new StoreEntities())
//            {
//                using (var transaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        //var product = db.Product.Find(id);
//                        db.Entry(_products[id]).State = EntityState.Deleted;
//                        db.SaveChanges();
//                        transaction.Commit();
//                        MessageBox.Show("Товар удален", "Успех", MessageBoxButton.OK);
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                            MessageBoxImage.Error);
//                    }
//                }
//            }
//        }
//    }
//}

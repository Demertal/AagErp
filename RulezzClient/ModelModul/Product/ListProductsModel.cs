using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace ModelModul.Product
{
    public class ListProductsModel : BindableBase
    {
        #region Properties

        private ObservableCollection<ProductModel> _products = new ObservableCollection<ProductModel>();

        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        #endregion

        public ListProductsModel()
        {
            Products = new ObservableCollection<ProductModel>();
        }

        #region LoadMethod

        public async Task<int> Load(int idGroup)
        {
            List<ProductModel> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.Products.Load();
                        return db.Products.Where(obj => obj.IdGroup == idGroup).Select(obj => new ProductModel
                        {
                            Id = obj.Id,
                            Title = obj.Title,
                            VendorCode = obj.VendorCode,
                            Barcode = obj.Barcode,
                            PurchasePrice = obj.PurchasePrice,
                            SalesPrice = obj.SalesPrice,
                            IdUnitStorage = obj.IdUnitStorage,
                            IdExchangeRate = obj.IdExchangeRate,
                            IdWarrantyPeriod = obj.IdWarrantyPeriod,
                            IdGroup = obj.IdGroup
                        }).ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<ProductModel> product = new ObservableCollection<ProductModel>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    product.Add(item);
                }
            }

            Products = product;
            return Products.Count;
        }

        public async Task<int> LoadByFindString(string findString)
        {
            List<ProductModel> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.Products.Load();
                        return db.Products
                            .Where(obj =>
                                obj.Title.Contains(findString) || obj.VendorCode.Contains(findString) ||
                                obj.Barcode.Contains(findString)).Select(obj => new ProductModel
                            {
                                Id = obj.Id,
                                Title = obj.Title,
                                VendorCode = obj.VendorCode,
                                Barcode = obj.Barcode,
                                PurchasePrice = obj.PurchasePrice,
                                SalesPrice = obj.SalesPrice,
                                IdUnitStorage = obj.IdUnitStorage,
                                IdExchangeRate = obj.IdExchangeRate,
                                IdWarrantyPeriod = obj.IdWarrantyPeriod,
                                IdGroup = obj.IdGroup
                            }).ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });

            ObservableCollection<ProductModel> product = new ObservableCollection<ProductModel>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    product.Add(item);
                }
            }

            Products = product;
            return Products.Count;
        }

        #endregion

        public void Delete(int id)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var product = db.Products.Find(id);
                        db.Entry(product).State = EntityState.Deleted;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Add(ProductModel product)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Products.Add(product.ConvertToProducts());
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Update(ProductModel product)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Prism.Mvvm;
//using RulezzClient.Models;

//namespace RulezzClient.ViewModels
//{
//    class RevaluationProductVM : BindableBase, IEquatable<RevaluationProductVM>
//    {
//        private RevaluationProductModel _product;

//        public RevaluationProductVM()
//        {
//            _product = new RevaluationProductModel();
//        }

//        public RevaluationProductVM(RevaluationProductModel obj)
//        {
//            Product = obj;
//        }

//        public RevaluationProductModel Product
//        {
//            get => _product;
//            set
//            {
//                _product = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int Id
//        {
//            get => _product.Id;
//            set
//            {
//                _product.Id = value;
//                RaisePropertyChanged();
//            }
//        }
//        public string Title
//        {
//            get => _product.Title;
//            set
//            {
//                _product.Title = value;
//                RaisePropertyChanged();
//            }
//        }
//        public string VendorCode
//        {
//            get => _product.VendorCode;
//            set
//            {
//                _product.VendorCode = value;
//                RaisePropertyChanged();
//            }
//        }
//        public string Barcode
//        {
//            get => _product.Barcode;
//            set
//            {
//                _product.Barcode = value;
//                RaisePropertyChanged();
//            }
//        }
//        public decimal PurchasePrice
//        {
//            get => _product.PurchasePrice;
//            set
//            {
//                _product.PurchasePrice = value;
//                RaisePropertyChanged();
//            }
//        }
//        public decimal OldPurchasePrice
//        {
//            get => _product.OldPurchasePrice;
//            set
//            {
//                _product.OldPurchasePrice = value;
//                RaisePropertyChanged();
//            }
//        }
//        public decimal SalesPrice
//        {
//            get => _product.SalesPrice;
//            set
//            {
//                _product.SalesPrice = value;
//                RaisePropertyChanged();
//            }
//        }
//        public decimal OldSalesPrice
//        {
//            get => _product.SalesPrice;
//            set
//            {
//                _product.SalesPrice = value;
//                RaisePropertyChanged();
//            }
//        }
//        public int IdUnitStorage
//        {
//            get => _product.IdUnitStorage;
//            set
//            {
//                _product.IdUnitStorage = value;
//                RaisePropertyChanged();
//            }
//        }
//        public int IdExchangeRate
//        {
//            get => _product.IdExchangeRate;
//            set
//            {
//                _product.IdExchangeRate = value;
//                RaisePropertyChanged();
//            }
//        }
//        public int IdWarrantyPeriod
//        {
//            get => _product.IdWarrantyPeriod;
//            set
//            {
//                _product.IdWarrantyPeriod = value;
//                RaisePropertyChanged();
//            }
//        }
//        public bool Equals(RevaluationProductVM other)
//        {
//            return _product.Equals(other?.Product);
//        }

//        public static explicit operator RevaluationProductVM(PurchaseInvoiceProductVM obj)
//        {
//            return new RevaluationProductVM {
//                Id = obj.Id,
//                Title = obj.Title,
//                VendorCode = obj.VendorCode,
//                Barcode = obj.Barcode,
//                PurchasePrice = obj.PurchasePrice,
//                OldPurchasePrice = obj.OldPurchasePrice,
//                OldSalesPrice = obj.SalesPrice,
//                SalesPrice = obj.SalesPrice,
//                IdUnitStorage = obj.IdUnitStorage,
//                IdExchangeRate = obj.IdExchangeRate,
//                IdWarrantyPeriod = obj.IdWarrantyPeriod
//            };
//        }
//    }
//}

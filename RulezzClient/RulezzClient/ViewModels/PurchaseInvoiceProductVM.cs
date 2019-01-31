using System;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class PurchaseInvoiceProductVM : BindableBase, IEquatable<PurchaseInvoiceProductVM>
    {
        private PurchaseInvoiceProductModel _product;

        public PurchaseInvoiceProductVM()
        {

        }

        public PurchaseInvoiceProductVM(PurchaseInvoiceProductModel obj)
        {
            Product = obj;
        }

        public PurchaseInvoiceProductModel Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _product.Id;
            set
            {
                _product.Id = value;
                RaisePropertyChanged();
            }
        }
        public string Title
        {
            get => _product.Title;
            set
            {
                _product.Title = value;
                RaisePropertyChanged();
            }
        }
        public string VendorCode
        {
            get => _product.VendorCode;
            set
            {
                _product.VendorCode = value;
                RaisePropertyChanged();
            }
        }
        public string Barcode
        {
            get => _product.Barcode;
            set
            {
                _product.Barcode = value;
                RaisePropertyChanged();
            }
        }
        public int Count
        {
            get => _product.Count;
            set
            {
                _product.Count = value;
                RaisePropertyChanged();
            }
        }
        public decimal PurchasePrice
        {
            get => _product.PurchasePrice;
            set
            {
                _product.PurchasePrice = value;
                RaisePropertyChanged();
            }
        }
        public decimal SalesPrice
        {
            get => _product.SalesPrice;
            set
            {
                _product.SalesPrice = value;
                RaisePropertyChanged();
            }
        }
        public int IdNomenclatureSubGroup
        {
            get => _product.IdNomenclatureSubGroup;
            set
            {
                _product.IdNomenclatureSubGroup = value;
                RaisePropertyChanged();
            }
        }
        public int IdUnitStorage
        {
            get => _product.IdUnitStorage;
            set
            {
                _product.IdUnitStorage = value;
                RaisePropertyChanged();
            }
        }
        public int IdExchangeRate
        {
            get => _product.IdExchangeRate;
            set
            {
                _product.IdExchangeRate = value;
                RaisePropertyChanged();
            }
        }
        public int IdWarrantyPeriod
        {
            get => _product.IdWarrantyPeriod;
            set
            {
                _product.IdWarrantyPeriod = value;
                RaisePropertyChanged();
            }
        }
        public decimal OldPurchasePrice
        {
            get => _product.OldPurchasePrice;
            set
            {
                _product.OldPurchasePrice = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(PurchaseInvoiceProductVM other)
        {
            return _product.Equals(other?.Product);
        }
    }
}

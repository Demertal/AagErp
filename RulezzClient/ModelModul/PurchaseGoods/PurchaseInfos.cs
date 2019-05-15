using System;
using Prism.Mvvm;

namespace ModelModul
{
    public partial class PurchaseInfos: BindableBase, IEquatable<PurchaseInfos>, ICloneable
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                RaisePropertyChanged();
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                RaisePropertyChanged();
            }
        }

        private int _idPurchaseReport;
        public int IdPurchaseReport
        {
            get => _idPurchaseReport;
            set
            {
                _idPurchaseReport = value;
                RaisePropertyChanged();
            }
        }

        private int _idProduct;
        public int IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                RaisePropertyChanged();
            }
        }

        private int _idExchangeRate;
        public int IdExchangeRate
        {
            get => _idExchangeRate;
            set
            {
                _idExchangeRate = value;
                RaisePropertyChanged();
            }
        }

        private ExchangeRates _exchangeRates;
        public virtual ExchangeRates ExchangeRates
        {
            get => _exchangeRates;
            set
            {
                _exchangeRates = value;
                RaisePropertyChanged();
            }
        }

        private Products _products;
        public virtual Products Products
        {
            get => _products;
            set
            {
                _products = value;
                RaisePropertyChanged();
            }
        }

        private PurchaseReports _purchaseReports;
        public virtual PurchaseReports PurchaseReports
        {
            get => _purchaseReports;
            set
            {
                _purchaseReports = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(PurchaseInfos other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && IdProduct == other.IdProduct && PurchasePrice == other.PurchasePrice &&
                   IdPurchaseReport == other.PurchasePrice && Count == other.Count && IdExchangeRate == other.IdExchangeRate;
        }

        public object Clone()
        {
            return new PurchaseInfos
            {
                Id = Id,
                Products = (Products) Products.Clone(),
                Count = Count,
                PurchasePrice = PurchasePrice,
                IdExchangeRate = IdExchangeRate,
                IdProduct = IdProduct,
                ExchangeRates = (ExchangeRates) ExchangeRates.Clone()
            };
        }
    }
}

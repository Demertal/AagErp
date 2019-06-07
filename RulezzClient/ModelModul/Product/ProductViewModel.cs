using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Prism.Mvvm;

namespace ModelModul.Product
{
    public class ProductViewModel: BindableBase
    {
        #region Properties

        private Products _product = new Products();

        public virtual Products Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Id");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("VendorCode");
                RaisePropertyChanged("Barcode");
                RaisePropertyChanged("SalesPrice");
                RaisePropertyChanged("IdWarrantyPeriod");
                RaisePropertyChanged("IdGroup");
                RaisePropertyChanged("IdUnitStorage");
                RaisePropertyChanged("Group");
                RaisePropertyChanged("CountProducts");
                RaisePropertyChanged("UnitStorage");
                RaisePropertyChanged("WarrantyPeriod");
                RaisePropertyChanged("PropertyProducts");
                RaisePropertyChanged("PurchaseInfos");
                RaisePropertyChanged("RevaluationProductsInfos");
                RaisePropertyChanged("SalesInfos");
                RaisePropertyChanged("SerialNumbers");
                RaisePropertyChanged("Count");
            }
        }

        public virtual int Id
        {
            get => Product.Id;
            set
            {
                Product.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get => Product.Title;
            set
            {
                Product.Title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }
        public string VendorCode
        {
            get => Product.VendorCode;
            set
            {
                Product.VendorCode = value;
                RaisePropertyChanged();
            }
        }
        public string Barcode
        {
            get => Product.Barcode;
            set
            {
                Product.Barcode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }
        public decimal SalesPrice
        {
            get => Product.SalesPrice;
            set
            {
                Product.SalesPrice = value;
                RaisePropertyChanged();
            }
        }
        public int IdWarrantyPeriod
        {
            get => Product.IdWarrantyPeriod;
            set
            {
                Product.IdWarrantyPeriod = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }
        public int IdGroup
        {
            get => Product.IdGroup;
            set
            {
                Product.IdGroup = value;
                RaisePropertyChanged();
            }
        }
        public int IdUnitStorage
        {
            get => Product.IdUnitStorage;
            set
            {
                Product.IdUnitStorage = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public Groups Group
        {
            get => Product.Groups;
            set
            {
                Product.Groups = value;
                RaisePropertyChanged();
            }
        }
        public ICollection<CountProducts> CountProducts
        {
            get => Product.CountProducts;
            set
            {
                Product.CountProducts = value;
                RaisePropertyChanged();
            }
        }
        public UnitStorages UnitStorage
        {
            get => Product.UnitStorages;
            set
            {
                Product.UnitStorages = value;
                RaisePropertyChanged();
            }
        }
        public WarrantyPeriods WarrantyPeriod
        {
            get => Product.WarrantyPeriods;
            set
            {
                Product.WarrantyPeriods = value;
                RaisePropertyChanged();
            }
        }
        public ICollection<PropertyProducts> PropertyProducts
        {
            get => Product.PropertyProducts;
            set
            {
                Product.PropertyProducts = value;
                RaisePropertyChanged();
            }
        }
        public ICollection<PurchaseInfos> PurchaseInfos
        {
            get => Product.PurchaseInfos;
            set
            {
                Product.PurchaseInfos = value;
                RaisePropertyChanged();
            }
        }

        public ICollection<RevaluationProductsInfos> RevaluationProductsInfos
        {
            get => Product.RevaluationProductsInfos;
            set
            {
                Product.RevaluationProductsInfos = value;
                RaisePropertyChanged();
            }
        }
        public ICollection<SalesInfos> SalesInfos
        {
            get => Product.SalesInfos;
            set
            {
                Product.SalesInfos = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SerialNumbers> SerialNumbers
        {
            get => Product.SerialNumbers as ObservableCollection<SerialNumbers>;
            set
            {
                Product.SerialNumbers = value;
                SerialNumbers.CollectionChanged += SerialNumbersCollectionChanged;
                RaisePropertyChanged();
            }
        }

        public virtual double Count
        {
            get
            {
                if (CountProducts == null || CountProducts.Count == 0) return 0;
                double count = 0;
                foreach (var ct in CountProducts)
                {
                    count += ct.Count;
                }
                return count;
            }
        }

        public bool IsValidate => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Barcode) && IdUnitStorage != 0 && IdWarrantyPeriod != 0;
        #endregion

        private void SerialNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (SerialNumbers item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= SerialNumbersViewModelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (SerialNumbers item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += SerialNumbersViewModelPropertyChanged;
                    }

                    break;
            }

            RaisePropertyChanged("IsValidate");
        }

        private void SerialNumbersViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("IsValidate");
        }
    }
}

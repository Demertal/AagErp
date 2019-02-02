using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class PurchaseInvoiceProductVM : BindableBase, IEquatable<PurchaseInvoiceProductVM>
    {
        private PurchaseInvoiceProductModel _product;

        private Visibility _serialNumbersVisibility;

        private readonly ObservableCollection<SerialNumberVM> _serialNumbers = new ObservableCollection<SerialNumberVM>();

        private readonly ReadOnlyObservableCollection<SerialNumberVM> _serial;

        public ReadOnlyObservableCollection<SerialNumberVM> SerialNumbers => _serial;

        public PurchaseInvoiceProductVM()
        {
            _serialNumbers.CollectionChanged += this.OnCollectionChanged;
            _serial = new ReadOnlyObservableCollection<SerialNumberVM>(_serialNumbers);
        }

        public PurchaseInvoiceProductVM(PurchaseInvoiceProductModel obj)
        {
            _serialNumbers.CollectionChanged += this.OnCollectionChanged;
            _serial = new ReadOnlyObservableCollection<SerialNumberVM>(_serialNumbers);
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

        public Visibility SerialNumbersVisibility => _product.IdWarrantyPeriod == 1 ? Visibility.Collapsed : Visibility.Visible;

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
                if (IdWarrantyPeriod != 1)
                {
                    while (_serialNumbers.Count < Count)
                    {
                        _serialNumbers.Add(new SerialNumberVM(new SerialNumber{IdProduct = Id, PurchaseDate = DateTime.Today, Value = ""}));
                    }

                    while (_serialNumbers.Count > Count)
                    {
                        bool remove = false;
                        foreach (var serial in _serialNumbers)
                        {
                            if (serial.Value != "") continue;
                            _serialNumbers.Remove(serial);
                            remove = true;
                            break;
                        }
                        if(remove) continue;
                        _serialNumbers.RemoveAt(_serialNumbers.Count-1);
                    }
                    RaisePropertyChanged("SerialNumbers");
                }
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

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (SerialNumberVM item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SerialNumberVM item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            RaisePropertyChanged("SerialNumbers");
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SerialNumbers");
        }
    }
}

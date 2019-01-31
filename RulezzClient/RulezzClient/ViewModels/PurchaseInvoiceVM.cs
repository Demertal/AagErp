using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class PurchaseInvoiceVM : BindableBase
    {
        private PurchaseInvoiceProductVM _selectedProducts;

        private int _selectedIndexProducts;

        private bool _isEnabledPurchaseInvoice;

        private readonly IUiDialogService _dialogService = new DialogService();

        private readonly ReadOnlyObservableCollection<PurchaseInvoiceProductVM> _rproducts;

        public ReadOnlyObservableCollection<PurchaseInvoiceProductVM> Products => _rproducts;

        public PurchaseInvoiceVM(Window wnd)
        {
            AllProduct.CollectionChanged += this.OnCollectionChanged;
            _rproducts = new ReadOnlyObservableCollection<PurchaseInvoiceProductVM>(AllProduct);
            AddProductComand = new DelegateCommand(() =>
            {
                object[] param = new object[1];
                param[0] = this;
                _dialogService.ShowDialog(DialogService.ChoiceView.ProductSelection, param, true, b => { });
            });
            PurchaseInvoiceCommand = new DelegateCommand(() =>
            {
                bool sucsuccess = false;
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //foreach (var item in AllProduct)
                            //{
                            //    Product product = db.Product.Find(item.Id);
                            //    product.SalesPrice = item.SalesPrice;
                            //    db.Entry(product).State = EntityState.Modified;
                            //    RevaluationProduct rev = new RevaluationProduct
                            //    {
                            //        IdProduct = product.Id,
                            //        Date = DateTime.Today,
                            //        NewSalesPrice = item.SalesPrice,
                            //        OldSalesPrice = item.OldSalesPrice
                            //    };
                            //    db.RevaluationProduct.Add(rev);
                            //}
                            //db.SaveChanges();
                            //transaction.Commit();
                            //sucsuccess = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        if (!sucsuccess) return;
                        MessageBox.Show("Товар переоценен", "Успех", MessageBoxButton.OK);
                        wnd.Close();
                    }
                }
            });
            DeleteProduct = new DelegateCommand(() =>
            {
                AllProduct.RemoveAt(SelectedIndexProducts);
            });
            //CellChanged = new DelegateCommand(() =>
            //{
            //    if (SelectedProducts.PurchasePrice == 0) IsEnabledPurchaseInvoice = false;
            //});
        }

        public ObservableCollection<PurchaseInvoiceProductVM> AllProduct { get; } = new ObservableCollection<PurchaseInvoiceProductVM>();

        public bool IsEnabledPurchaseInvoice
        {
            get
            {
                if (AllProduct.Count == 0) return false;
                foreach (var pr in AllProduct)
                {
                    if (pr.PurchasePrice == 0 || pr.Count == 0) return false;
                }

                return true;
            }
        }

        public int SelectedIndexProducts
        {
            get => _selectedIndexProducts;
            set
            {
                _selectedIndexProducts = value;
                RaisePropertyChanged();
            }
        }

        public PurchaseInvoiceProductVM SelectedProducts
        {
            get => _selectedProducts;
            set
            {
                _selectedProducts = value;
                RaisePropertyChanged();
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (PurchaseInvoiceProductVM item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PurchaseInvoiceProductVM item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            RaisePropertyChanged("IsEnabledPurchaseInvoice");
        }

        public DelegateCommand AddProductComand { get; }
        public DelegateCommand PurchaseInvoiceCommand { get; }
        public DelegateCommand DeleteProduct { get; }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("IsEnabledPurchaseInvoice");
        }
    }
}
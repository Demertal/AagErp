//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Data.Entity;
//using System.Linq;
//using System.Windows;
//using Prism.Commands;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class PurchaseInvoiceVM : BindableBase
//    {
//        private Stores _selectedStore;
//        private PurchaseInvoiceProductVM _selectedProducts;
//        private int _selectedIndexProducts;
//        private bool _isEnabledPurchaseInvoice;
//        private Visibility _isRevaluationVisibility;
//        private readonly IUiDialogService _dialogService = new DialogService();

//        private readonly ReadOnlyObservableCollection<PurchaseInvoiceProductVM> _purchaseInvoiceProducts;
//        private readonly ReadOnlyObservableCollection<RevaluationProductVM> _revaluationProducts;

//        public StoreListVm StoreList = new StoreListVm();

//        public ReadOnlyObservableCollection<PurchaseInvoiceProductVM> PurchaseInvoiceProducts => _purchaseInvoiceProducts;
//        public ReadOnlyObservableCollection<RevaluationProductVM> RevaluationProducts => _revaluationProducts;
//        public ReadOnlyObservableCollection<Stores> Stores => StoreList.Stores;

//        public PurchaseInvoiceVM(Window wnd)
//        {
//            StoreList.Load();
//            PurchaseInvoiceConverterParameter = false;
//            IsRevaluationVisibility = Visibility.Collapsed;
//            PurchaseInvoiceProduct.CollectionChanged += this.OnCollectionChanged;
//            _purchaseInvoiceProducts = new ReadOnlyObservableCollection<PurchaseInvoiceProductVM>(PurchaseInvoiceProduct);
//            _revaluationProducts = new ReadOnlyObservableCollection<RevaluationProductVM>(RevaluationProduct);
//            AddProductComand = new DelegateCommand(() =>
//            {
//                object[] param = new object[1];
//                param[0] = this;
//                _dialogService.ShowDialog(DialogService.ChoiceView.ProductSelection, param, true, b => { });
//            });
//            PurchaseInvoiceCommand = new DelegateCommand(() =>
//            {
//                bool sucsuccess = false;
//                using (StoreEntities db = new StoreEntities())
//                {
//                    using (var transaction = db.Database.BeginTransaction())
//                    {
//                        try
//                        {
//                            PurchaseReports report = new PurchaseReports();
//                            report.DataOrder = DateTime.Today;
//                            report.IdStore = SelectedStore.Id;


//                            foreach (var item in RevaluationProduct)
//                            {
//                                if (item.OldSalesPrice == item.SalesPrice) continue;
//                                Products product = db.Products.Find(item.Id);
//                                if (product == null) throw new Exception("Товар не найден");
//                                product.SalesPrice = item.SalesPrice;
//                                db.Entry(product).State = EntityState.Modified;
//                                RevaluationProducts rev = new RevaluationProducts
//                                {
//                                    IdProduct = product.Id,
//                                    Date = DateTime.Today,
//                                    NewSalesPrice = item.SalesPrice,
//                                    OldSalesPrice = item.OldSalesPrice
//                                };
//                                db.RevaluationProducts.Add(rev);
//                            }
//                            foreach (var item in PurchaseInvoiceProduct)
//                            {
//                                Products product = db.Products.Find(item.Id);
//                                if (product == null) throw new Exception("Товар не найден");
//                                product.PurchasePrice = item.PurchasePrice;
//                                //product.Count += item.Count;
//                                db.Entry(product).State = EntityState.Modified;
//                                PurchaseInfos pur = new PurchaseInfos
//                                {
//                                    IdProduct = product.Id
//                                    //OldSalesPrice = item.OldSalesPrice
//                                };
//                                //db.RevaluationProduct.Add(pur);
//                            }
//                            db.SaveChanges();
//                            transaction.Commit();
//                            sucsuccess = true;
//                        }
//                        catch (Exception ex)
//                        {
//                            transaction.Rollback();
//                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                                MessageBoxImage.Error);
//                        }
//                        if (!sucsuccess) return;
//                        MessageBox.Show("Товар переоценен", "Успех", MessageBoxButton.OK);
//                        wnd.Close();
//                    }
//                }
//            });
//            DeleteProduct = new DelegateCommand<Collection<object>>(obj =>
//            {
//                List<PurchaseInvoiceProductVM> list = obj.Cast<PurchaseInvoiceProductVM>().ToList();
//                list.ForEach(item => PurchaseInvoiceProduct.Remove(item));
//            });
//        }

//        public Stores SelectedStore
//        {
//            get => _selectedStore;
//            set
//            {
//                _selectedStore = value;
//                RaisePropertyChanged();
//            }
//        }

//        public bool PurchaseInvoiceConverterParameter { get; set; }

//        public Visibility IsRevaluationVisibility
//        {
//            get => _isRevaluationVisibility;
//            set
//            {
//                _isRevaluationVisibility = value;
//                RaisePropertyChanged();
//            }
//        }
//        public ObservableCollection<PurchaseInvoiceProductVM> PurchaseInvoiceProduct { get; } = new ObservableCollection<PurchaseInvoiceProductVM>();
//        public ObservableCollection<RevaluationProductVM> RevaluationProduct { get; } = new ObservableCollection<RevaluationProductVM>();

//        public bool IsEnabledPurchaseInvoice
//        {
//            get => _isEnabledPurchaseInvoice;
//            set
//            {
//                _isEnabledPurchaseInvoice = value;
//                RaisePropertyChanged();
//            }
//        }

//        public int SelectedIndexProducts
//        {
//            get => _selectedIndexProducts;
//            set
//            {
//                _selectedIndexProducts = value;
//                RaisePropertyChanged();
//            }
//        }

//        public PurchaseInvoiceProductVM SelectedProducts
//        {
//            get => _selectedProducts;
//            set
//            {
//                _selectedProducts = value;
//                RaisePropertyChanged();
//            }
//        }

//        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.Action == NotifyCollectionChangedAction.Remove)
//            {
//                foreach (PurchaseInvoiceProductVM item in e.OldItems)
//                {
//                    //Removed items
//                    item.PropertyChanged -= EntityViewModelPropertyChanged;
//                    RevaluationProductVM rev = RevaluationProduct.FirstOrDefault(ob => ob.Id == item.Id);
//                    RevaluationProduct.Remove(rev);
//                }
//            }
//            else if (e.Action == NotifyCollectionChangedAction.Add)
//            {
//                foreach (PurchaseInvoiceProductVM item in e.NewItems)
//                {
//                    //Added items
//                    item.PropertyChanged += EntityViewModelPropertyChanged;
//                }
//            }
//            IsRevaluationVisibility = RevaluationProduct.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
//            RaisePropertyChanged("IsEnabledPurchaseInvoice");
//        }

//        public DelegateCommand AddProductComand { get; }
//        public DelegateCommand PurchaseInvoiceCommand { get; }
//        public DelegateCommand<Collection<object>> DeleteProduct { get; }

//        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (sender != null)
//            {
//                RevaluationProductVM rev = RevaluationProduct.FirstOrDefault(ob => ob.Id == (sender as PurchaseInvoiceProductVM)?.Id);
//                if (rev != null && rev.Id == (sender as PurchaseInvoiceProductVM)?.Id)
//                {
//                    RevaluationProduct[RevaluationProduct.IndexOf(rev)].PurchasePrice = ((PurchaseInvoiceProductVM)sender).PurchasePrice;
//                    if (((PurchaseInvoiceProductVM) sender).PurchasePrice ==
//                      ((PurchaseInvoiceProductVM) sender).OldPurchasePrice &&
//                      ((PurchaseInvoiceProductVM) sender).SalesPrice != 0 || ((PurchaseInvoiceProductVM)sender).PurchasePrice == 0)
//                    {
//                        RevaluationProduct.Remove(rev);
//                    }
//                }
//                else
//                {
//                    if ((sender as PurchaseInvoiceProductVM)?.PurchasePrice !=
//                        (sender as PurchaseInvoiceProductVM)?.OldPurchasePrice ||
//                        (sender as PurchaseInvoiceProductVM)?.SalesPrice == 0)
//                    {
//                        RevaluationProduct.Add((RevaluationProductVM)(PurchaseInvoiceProductVM)sender);
//                    }
//                }
//            }

//            IsEnabledPurchaseInvoice = true;
//            if (PurchaseInvoiceProduct.Count == 0)
//            {
//                IsEnabledPurchaseInvoice = false;
//            }
//            foreach (var pr in PurchaseInvoiceProduct)
//            {
//                if (pr.PurchasePrice == 0 || pr.Count == 0)
//                {
//                    IsEnabledPurchaseInvoice = false;
//                    break;
//                }
//                foreach (var ser in pr.SerialNumbers)
//                {
//                    if (ser.Value == String.Empty)
//                    {
//                        IsEnabledPurchaseInvoice = false;
//                        break;
//                    }
//                }
//            }
//            IsRevaluationVisibility = RevaluationProduct.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
//            RaisePropertyChanged("RevaluationProducts");
//        }
//    }
//}
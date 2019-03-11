//using System;
//using System.Collections;
//using System.Collections.ObjectModel;
//using System.Windows;
//using System.Windows.Controls;
//using Prism.Commands;
//using Prism.Mvvm;
//using RulezzClient.Models;
//using RulezzClient.Properties;
//using RulezzClient.Views;

//namespace RulezzClient.ViewModels
//{
//    class ShowProductViewModel : BindableBase
//    {
//        public enum ChoiceUpdate : byte
//        {
//            Store,
//            NomenclatureGroup,
//            NomenclatureSubgroup,
//            Product
//        }

//        #region Parametr

//        private bool _enableFilter;
//        private readonly IUiDialogService _dialogService = new DialogService();
//        private ProductView _selectedProduct;
//        private Visibility _isSelectedProduct;
//        private Visibility _isNotSelectedProduct;
//        private SelectionMode _selectionMode;
//        private Visibility _isEnableFilter;
//        private ShowStructurVM _showStructur;
//        private string _findString;
        
//        #endregion

//        public StoreListVm StoreList = new StoreListVm();
//        public ProductListVm ProductList = new ProductListVm();

//        public ReadOnlyObservableCollection<ProductView> Products => ProductList.Products;

//        public ShowProductViewModel()
//        {
//            ShowStructur = new ShowStructurVM();
//            SelectionMode = SelectionMode.Single;
//            IsSelectedProduct = Visibility.Collapsed;
//            IsNotSelectedProduct = Visibility.Visible;
//            SelectedProductCommand = null;
//            Update(ChoiceUpdate.Store);
//            FindString = "";
//            DeleteProduct = new DelegateCommand(() =>
//            {
//                if (MessageBox.Show("Вы уверены что хотите удалить товар?", "Удаление", MessageBoxButton.YesNo,
//                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
//                try
//                {
//                    ProductList.Delete(SelectedProduct.Id);
//                    Update(ChoiceUpdate.Product);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                        MessageBoxImage.Error);
//                }
//            });
//            UpdateProduct = new DelegateCommand(() =>
//            {
//                if (SelectedProduct == null) return;
//                object []param = new object[1];
//                param[0] = SelectedProduct;
//                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateProduct, param, true, b => { });
//                Update(ChoiceUpdate.Product);
//            });
//        }

//        public ShowProductViewModel(object obj, Window wnd)
//        {
//            SelectionMode = SelectionMode.Multiple;
//            IsSelectedProduct = Visibility.Visible;
//            IsNotSelectedProduct = Visibility.Collapsed;
//            Update(ChoiceUpdate.Store);
//            FindString = "";
//            DeleteProduct = null;
//            UpdateProduct = new DelegateCommand(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    if (obj.GetType().ToString() == "RulezzClient.ViewModels.RevaluationVM")
//                    {
//                        RevaluationProductModel revItem =
//                            new RevaluationProductModel(db.Products.Find(SelectedProduct?.Id));
//                        if (!(obj as RevaluationVM).AllProduct.Contains(revItem)) (obj as RevaluationVM).AllProduct.Add(revItem);
//                    }
//                    else if (obj.GetType().ToString() == "RulezzClient.ViewModels.PurchaseInvoiceVM")
//                    {
//                        PurchaseInvoiceProductVM purItem =
//                            new PurchaseInvoiceProductVM(
//                                new PurchaseInvoiceProductModel(db.Products.Find(SelectedProduct?.Id)));
//                        if (!(obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Contains(purItem)) (obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Add(purItem);
//                    }
//                }
//                wnd.Close();
//            });
//            SelectedProductCommand = new DelegateCommand<object>(sel =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    foreach (var item in (IList)sel)
//                    {
//                        if (obj.GetType().ToString() == "RulezzClient.ViewModels.RevaluationVM")
//                        {
//                            RevaluationProductModel revItem =
//                                new RevaluationProductModel(db.Products.Find((item as ProductView)?.Id));
//                            if ((obj as RevaluationVM).AllProduct.Contains(revItem)) continue;
//                            (obj as RevaluationVM).AllProduct.Add(revItem);
//                        }
//                        else if (obj.GetType().ToString() == "RulezzClient.ViewModels.PurchaseInvoiceVM")
//                        {
//                            PurchaseInvoiceProductVM purItem =
//                                new PurchaseInvoiceProductVM(
//                                    new PurchaseInvoiceProductModel(db.Products.Find((item as ProductView)?.Id)));
//                            if ((obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Contains(purItem)) continue;
//                            (obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Add(purItem);
//                        }
//                    }
//                }
//                wnd.Close();
//            });
//        }

//        #region GetSetMethod

//        public string FindString
//        {
//            get => _findString;
//            set
//            {
//                _findString = value;
//                RaisePropertyChanged();
//                Update(ChoiceUpdate.Product);
//            }
//        }

//        public SelectionMode SelectionMode
//        {
//            get => _selectionMode;
//            set
//            {
//                _selectionMode = value;
//                RaisePropertyChanged();
//            }
//        }

//        public ShowStructurVM ShowStructur
//        {
//            get => _showStructur;
//            set
//            {
//                _showStructur = value;
//                RaisePropertyChanged();
//            }
//        }

//        public ProductView SelectedProduct
//        {
//            get => _selectedProduct;
//            set
//            {
//                _selectedProduct = value;
//                RaisePropertyChanged();
//            }
//        }

//        public Visibility IsSelectedProduct
//        {
//            get => _isSelectedProduct;
//            set
//            {
//                _isSelectedProduct = value;
//                RaisePropertyChanged();
//            }
//        }

//        public Visibility IsNotSelectedProduct
//        {
//            get => _isNotSelectedProduct;
//            set
//            {
//                _isNotSelectedProduct = value;
//                RaisePropertyChanged();
//            }
//        }

//        #endregion

//        public async void Update(ChoiceUpdate choice)
//        {
//            if (FindString == "")
//            {
//                //if (_selectedNomenclatureSubGroup == null) await ProductList.LoadByNomenclatureSubGroup(-1);
//                //else await ProductList.LoadByNomenclatureSubGroup(_selectedNomenclatureSubGroup.Id);
//            }
//            else
//            {
//                int idGroup = -1;
//                if (ShowStructur?.SelectedNode != null)
//                {
//                    idGroup = ShowStructur.SelectedNode.Group.Id;
//                }
//                await ProductList.Load(idGroup);
//            }
//            RaisePropertyChanged("Products");
//        }

//        public DelegateCommand DeleteProduct { get; }

//        public DelegateCommand UpdateProduct { get; }

//        public DelegateCommand<object> SelectedProductCommand { get; }
//    }
//}

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;
using RulezzClient.Properties;

namespace RulezzClient.ViewModels
{
    class ShowProductViewModel : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            Product
        }

        private bool _enableFilter;
        private readonly IUiDialogService _dialogService = new DialogService();
        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubGroup _selectedNomenclatureSubGroup;
        private ProductView _selectedProduct;
        private Visibility _isSelectedProduct;
        private Visibility _isNotSelectedProduct;
        private SelectionMode _selectionMode;
        private Visibility _isEnableFilter;
        private string _findString;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public ProductListVm ProductList = new ProductListVm();

        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups => NomenclatureSubgroupList.NomenclatureSubGroups;
        public ReadOnlyObservableCollection<ProductView> Products => ProductList.Products;

        public ShowProductViewModel()
        {
            FindString = "";
            EnableFilter = false;
            SelectionMode = SelectionMode.Single;
            IsSelectedProduct = Visibility.Collapsed;
            IsNotSelectedProduct = Visibility.Visible;
            SelectedProductCommand = null;
            StoreList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureGroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureSubgroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ProductList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            Update(ChoiceUpdate.Store);
            DeleteProduct = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Вы уверены что хотите удалить товар?", "Удаление", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    ProductList.Delete(SelectedProduct.Id);
                    Update(ChoiceUpdate.Product);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
            UpdateProduct = new DelegateCommand(() =>
            {
                if (SelectedProduct == null) return;
                object []param = new object[4];
                param[0] = SelectedProduct;
                param[1] = SelectedNomenclatureSubGroup;
                param[2] = SelectedNomenclatureGroup;
                param[3] = SelectedStore;
                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateProduct, param, true, b => { });
                Update(ChoiceUpdate.Product);
            });
        }

        public ShowProductViewModel(RevaluationVM rev, Window wnd)
        {
            FindString = "";
            EnableFilter = false;
            SelectionMode = SelectionMode.Multiple;
            IsSelectedProduct = Visibility.Visible;
            IsNotSelectedProduct = Visibility.Collapsed;
            StoreList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureGroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureSubgroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ProductList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            Update(ChoiceUpdate.Store);
            DeleteProduct = null;
            UpdateProduct = null;
            SelectedProductCommand = new DelegateCommand<object>(sel =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    foreach (var item in (IList)sel)
                    {
                        RevaluationProductModel revItem = new RevaluationProductModel(db.Product.Find((item as ProductView)?.Id));
                        if(rev.AllProduct.Contains(revItem))continue;
                        rev.AllProduct.Add(revItem);
                    }
                }
                wnd.Close();
            });
        }

        public string FindString
        {
            get => _findString;
            set
            {
                _findString = value;
                RaisePropertyChanged();
                Update(ChoiceUpdate.Product);
            }
        }

        public bool EnableFilter
        {
            get => _enableFilter;
            set
            {
                _enableFilter = value;
                if (_enableFilter)
                {
                    IsEnableFilter = Visibility.Visible;
                    Update(ChoiceUpdate.NomenclatureGroup);
                }
                else
                {
                    IsEnableFilter = Visibility.Collapsed;
                    Update(ChoiceUpdate.Product);
                }
                RaisePropertyChanged();
            }
        }

        public Visibility IsEnableFilter
        {
            get => _isEnableFilter;
            set
            {
                _isEnableFilter = value;
                RaisePropertyChanged();
            }
        }

        public SelectionMode SelectionMode
        {
            get => _selectionMode;
            set
            {
                _selectionMode = value;
                RaisePropertyChanged();
            }
        }

        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                Settings.Default.SelectedStoreID = _selectedStore.Id;
                Update(EnableFilter ? ChoiceUpdate.NomenclatureGroup : ChoiceUpdate.Product);
                RaisePropertyChanged();
            }
        }

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                Update(ChoiceUpdate.NomenclatureSubgroup);
                RaisePropertyChanged();
            }
        }

        public NomenclatureSubGroup SelectedNomenclatureSubGroup
        {
            get => _selectedNomenclatureSubGroup;
            set
            {
                _selectedNomenclatureSubGroup = value;
                Update(ChoiceUpdate.Product);
                RaisePropertyChanged();
            }
        }

        public ProductView SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
            }
        }

        public Visibility IsSelectedProduct
        {
            get => _isSelectedProduct;
            set
            {
                _isSelectedProduct = value;
                RaisePropertyChanged();
            }
        }

        public Visibility IsNotSelectedProduct
        {
            get => _isNotSelectedProduct;
            set
            {
                _isNotSelectedProduct = value;
                RaisePropertyChanged();
            }
        }

        private void CheckSelectedStore()
        {
            if (SelectedStore == null)
            {
                if (Stores.Count != 0)
                {
                    SelectedStore = Stores[0];
                }
            }
            else
            {
                if (!Stores.Contains(_selectedStore)) SelectedStore = null;
            }
        }

        private void CheckSelectedSelectedNomenclatureGroup()
        {
            if (SelectedNomenclatureGroup == null)
            {
                if (NomenclatureGroups.Count != 0)
                {
                    SelectedNomenclatureGroup = NomenclatureGroups[0];
                }
            }
            else
            {
                if (!NomenclatureGroups.Contains(SelectedNomenclatureGroup)) SelectedNomenclatureGroup = null;
            }
        }

        private void CheckSelectedSelectedNomenclatureSubgroup()
        {
            if (SelectedNomenclatureSubGroup == null)
            {
                if (NomenclatureSubGroups.Count != 0)
                {
                    SelectedNomenclatureSubGroup = NomenclatureSubGroups[0];
                }
            }
            else
            {
                if (!NomenclatureSubGroups.Contains(SelectedNomenclatureSubGroup)) SelectedNomenclatureSubGroup = null;
            }
        }

        public async void Update(ChoiceUpdate choice)
        {
            switch (choice)
            {
                case ChoiceUpdate.Store:
                    await StoreList.Load();
                    CheckSelectedStore();
                    break;
                case ChoiceUpdate.NomenclatureGroup:
                    if (_selectedStore == null) await NomenclatureGroupList.Load(-1);
                    else await NomenclatureGroupList.Load(Settings.Default.SelectedStoreID);
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.Load(-1);
                    else await NomenclatureSubgroupList.Load(_selectedNomenclatureGroup.Id);
                    CheckSelectedSelectedNomenclatureSubgroup();
                    break;
                case ChoiceUpdate.Product:
                    if (EnableFilter)
                    {
                        if (FindString == "")
                        {
                            if (_selectedNomenclatureSubGroup == null) await ProductList.LoadByNomenclatureSubGroup(-1);
                            else await ProductList.LoadByNomenclatureSubGroup(_selectedNomenclatureSubGroup.Id);
                        }
                        else
                        {
                            if (_selectedStore == null) await ProductList.LoadByFindString(-1, FindString);
                            else await ProductList.LoadByFindString(Settings.Default.SelectedStoreID, FindString);
                        }
                    }
                    else
                    {
                        if (_selectedStore == null) await ProductList.LoadAll(-1);
                        else await ProductList.LoadAll(Settings.Default.SelectedStoreID);
                    }
                    RaisePropertyChanged("Products");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }

        public DelegateCommand DeleteProduct { get; }

        public DelegateCommand UpdateProduct { get; }

        public DelegateCommand<object> SelectedProductCommand { get; }
    }
}

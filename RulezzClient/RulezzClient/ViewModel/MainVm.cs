using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient
{
    class MainVm : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            Product
        }
        private Visibility _isProductGroupVisible;
        private readonly string _connectionString;

        private StoreVm _selectedStore;
        private NomenclatureGroupVm _selectedNomenclatureGroup;
        private NomenclatureSubgroupVm _selectedNomenclatureSubgroup;
        private ProductVm _selectedProduct;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public ProductListVm ProductList = new ProductListVm();

        public ReadOnlyObservableCollection<StoreVm> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroupVm> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubgroupVm> NomenclatureSubgroups => NomenclatureSubgroupList.NomenclatureSubgroups;
        public ReadOnlyObservableCollection<ProductVm> Products => ProductList.Products;

        public MainVm(string connectionString)
        {
            _connectionString = connectionString;
            _isProductGroupVisible = Visibility.Collapsed;
            StoreList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureGroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureSubgroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ProductList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ShowProductCommand = new DelegateCommand(() =>
            {
                IsProductGroupVisible = Visibility.Visible;
                Update(ChoiceUpdate.Store);
            });
            //AddProductMainCommand = new DelegateCommand(() =>
            //{
            //    AddProduct ad = new AddProduct { DataContext = new VmAddProduct(_model), Title = "Добавить товар" };
            //    ad.ShowDialog();
            //});
        }

        public Visibility IsProductGroupVisible
        {
            get => _isProductGroupVisible;
            set
            {
                _isProductGroupVisible = value;
                RaisePropertyChanged();
            }
        }

        public StoreVm SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                Update(ChoiceUpdate.NomenclatureGroup);
                RaisePropertyChanged();
            }
        }

        public NomenclatureGroupVm SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                Update(ChoiceUpdate.NomenclatureSubgroup);
                RaisePropertyChanged();
            }
        }

        public NomenclatureSubgroupVm SelectedNomenclatureSubgroup
        {
            get => _selectedNomenclatureSubgroup;
            set
            {
                _selectedNomenclatureSubgroup = value;
                Update(ChoiceUpdate.Product);
                RaisePropertyChanged();
            }
        }

        public ProductVm SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
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
            if (SelectedNomenclatureSubgroup == null)
            {
                if (NomenclatureSubgroups.Count != 0)
                {
                    SelectedNomenclatureSubgroup = NomenclatureSubgroups[0];
                }
            }
            else
            {
                if (!NomenclatureSubgroups.Contains(SelectedNomenclatureSubgroup)) SelectedNomenclatureSubgroup = null;
            }
        }

        public async void Update(ChoiceUpdate choice)
        {
            switch (choice)
            {
                case ChoiceUpdate.Store:
                    await StoreList.GetListStore(_connectionString);
                    CheckSelectedStore();
                    break;
                case ChoiceUpdate.NomenclatureGroup:
                    if (_selectedStore == null) await NomenclatureGroupList.GetListNomenclatureGroup(_connectionString, -1);
                    else await NomenclatureGroupList.GetListNomenclatureGroup(_connectionString, _selectedStore.Id);
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.GetListNomenclatureSubgroup(_connectionString, -1);
                    else await NomenclatureSubgroupList.GetListNomenclatureSubgroup(_connectionString, _selectedNomenclatureGroup.Id);
                    CheckSelectedSelectedNomenclatureSubgroup();
                    break;
                case ChoiceUpdate.Product:
                    if (_selectedNomenclatureSubgroup == null)  await ProductList.GetListProduct(_connectionString, -1);
                    else await ProductList.GetListProduct(_connectionString, _selectedNomenclatureSubgroup.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }

        public DelegateCommand ShowProductCommand { get; }

        //public DelegateCommand AddProductMainCommand { get; }
    }
}
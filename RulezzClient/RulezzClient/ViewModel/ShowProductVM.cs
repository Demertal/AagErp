using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace RulezzClient
{
    class ShowProductVm : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            Product
        }

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

        public ShowProductVm()
        {
            StoreList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureGroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            NomenclatureSubgroupList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ProductList.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            Update(ChoiceUpdate.Store);
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
                    await StoreList.GetListStore();
                    CheckSelectedStore();
                    break;
                case ChoiceUpdate.NomenclatureGroup:
                    if (_selectedStore == null) await NomenclatureGroupList.GetListNomenclatureGroup(-1);
                    else await NomenclatureGroupList.GetListNomenclatureGroup(_selectedStore.Id);
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.GetListNomenclatureSubgroup(-1);
                    else await NomenclatureSubgroupList.GetListNomenclatureSubgroup(_selectedNomenclatureGroup.Id);
                    CheckSelectedSelectedNomenclatureSubgroup();
                    break;
                case ChoiceUpdate.Product:
                    if (_selectedNomenclatureSubgroup == null) await ProductList.GetListProduct(-1);
                    else await ProductList.GetListProduct(_selectedNomenclatureSubgroup.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }
    }
}

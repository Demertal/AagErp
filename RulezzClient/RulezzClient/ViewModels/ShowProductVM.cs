using System;
using System.Collections.ObjectModel;
using System.Windows;
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

        private readonly IUiDialogService _dialogService = new DialogService();
        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubGroup _selectedNomenclatureSubGroup;
        private ProductView_Result _selectedProduct;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public ProductListVm ProductList = new ProductListVm();

        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups => NomenclatureSubgroupList.NomenclatureSubGroups;
        public ReadOnlyObservableCollection<ProductView_Result> Products => ProductList.Products;

        public ShowProductViewModel()
        {
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

        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                Settings.Default.SelectedStoreID = _selectedStore.Id;
                Update(ChoiceUpdate.NomenclatureGroup);
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

        public ProductView_Result SelectedProduct
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
                    if (_selectedNomenclatureSubGroup == null) await ProductList.Load(-1);
                    else await ProductList.Load(_selectedNomenclatureSubGroup.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }

        public DelegateCommand DeleteProduct { get; }

        public DelegateCommand UpdateProduct { get; }
    }
}

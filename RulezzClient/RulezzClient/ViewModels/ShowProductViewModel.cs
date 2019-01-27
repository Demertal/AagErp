using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using RulezzClient.Model;
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
        private NomenclatureSubgroup _selectedNomenclatureSubgroup;
        private Product _selectedProduct;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public ProductListVm ProductList = new ProductListVm();

        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups => NomenclatureSubgroupList.NomenclatureSubgroups;
        public ReadOnlyObservableCollection<Product> Products => ProductList.Products;

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
                    SelectedProduct.Delete(Settings.Default.СconnectionString);
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
                if(SelectedProduct == null) return;
                UpdateProductViewModel viewModel = new UpdateProductViewModel(SelectedProduct, SelectedNomenclatureSubgroup, SelectedNomenclatureGroup, SelectedStore);
                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateProduct, viewModel, true, b => { });
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

        public NomenclatureSubgroup SelectedNomenclatureSubgroup
        {
            get => _selectedNomenclatureSubgroup;
            set
            {
                _selectedNomenclatureSubgroup = value;
                Update(ChoiceUpdate.Product);
                RaisePropertyChanged();
            }
        }

        public Product SelectedProduct
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
                    else await NomenclatureGroupList.GetListNomenclatureGroup(Settings.Default.SelectedStoreID);
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

        public DelegateCommand DeleteProduct { get; }

        public DelegateCommand UpdateProduct { get; }
    }
}

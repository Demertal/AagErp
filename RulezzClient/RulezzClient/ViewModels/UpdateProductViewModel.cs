using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class UpdateProductViewModel : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            UnitStorage,
            WarrantyPeriod
        }

        private readonly Product _oldProduct;

        public string OldStore { get; }
        public string OldNomenclatureGroup { get; }
        public string OldNomenclatureSubgroup { get; }
        public string OldTitle => _oldProduct.Title;
        public string OldBarcode => _oldProduct.Barcode;
        public string OldVendorCode => _oldProduct.VendorCode;
        //public string OldUnitStorage => _oldProduct.UnitStorage;
        //public string OldWarranty => _oldProduct.Warranty;

        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
       // private NomenclatureSubgroup _selectedNomenclatureSubgroup;
        private UnitStorage _selectedUnitStorage;
        private WarrantyPeriod _selectedWarrantyPeriod;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public UnitStorageListVm UnitStorageList = new UnitStorageListVm();
        public WarrantyPeriodListVm WarrantyPeriodList = new WarrantyPeriodListVm();
        public Product Product = new Product();

        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        //public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubGroups => NomenclatureSubgroupList.NomenclatureSubGroups;
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages => UnitStorageList.UnitStorages;
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

        //public UpdateProductViewModel(Product product, NomenclatureSubgroup nomenclatureSubgroup, NomenclatureGroup nomenclatureGroup, Store store)
        //{
        //    _oldProduct = product;
        //    OldNomenclatureSubgroup = nomenclatureSubgroup.Title;
        //    OldNomenclatureGroup = nomenclatureGroup.Title;
        //    OldStore = store.Title;
        //    Update(AddProductViewModel.ChoiceUpdate.Store);
        //    Update(AddProductViewModel.ChoiceUpdate.WarrantyPeriod);
        //    Update(AddProductViewModel.ChoiceUpdate.UnitStorage);
        //    SelectedStore = new Store(store);
        //    SelectedNomenclatureGroup = new NomenclatureGroup(nomenclatureGroup);
        //    SelectedNomenclatureSubGroup = new NomenclatureSubgroup(nomenclatureSubgroup);
        //    //AddProduct = new DelegateCommand(() =>
        //    //{
        //    //    if (product.Add());
        //    //});
        //}

        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                Update(AddProductViewModel.ChoiceUpdate.NomenclatureGroup);
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                Update(AddProductViewModel.ChoiceUpdate.NomenclatureSubgroup);
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        //public NomenclatureSubgroup SelectedNomenclatureSubGroup
        //{
        //    get => _selectedNomenclatureSubgroup;
        //    set
        //    {
        //        _selectedNomenclatureSubgroup = value;
        //        if (_selectedNomenclatureSubgroup != null) Product.IdNomenclatureSubgroup = _selectedNomenclatureSubgroup.Id;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged("IsButtonAddEnabled");
        //    }
        //}

        public UnitStorage SelectedUnitStorage
        {
            get => _selectedUnitStorage;
            set
            {
                _selectedUnitStorage = value;
                //if (_selectedUnitStorage != null) Product.UnitStorage = _selectedUnitStorage.Id.ToString();
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public WarrantyPeriod SelectedWarrantyPeriod
        {
            get => _selectedWarrantyPeriod;
            set
            {
                _selectedWarrantyPeriod = value;
               // if (_selectedWarrantyPeriod != null) Product.Warranty = _selectedWarrantyPeriod.Id.ToString();
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

       // public bool IsButtonAddEnabled => SelectedStore != null && SelectedNomenclatureGroup != null && SelectedNomenclatureSubGroup != null && SelectedUnitStorage != null && SelectedWarrantyPeriod != null && Product.Title != null && Product.Title != "" && Product.Title != "" && Product.VendorCode.Length <= 20 && Product.Barcode.Length <= 13;

        public string Title
        {
            get => Product.Title;
            set
            {
                Product.Title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public string Barcode
        {
            get => Product.Barcode;
            set
            {
                Product.Barcode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public string VendorCode
        {
            get => Product.VendorCode;
            set
            {
                Product.VendorCode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
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

        //private void CheckSelectedSelectedNomenclatureSubgroup()
        //{
        //    if (SelectedNomenclatureSubGroup == null)
        //    {
        //        if (NomenclatureSubGroups.Count != 0)
        //        {
        //            SelectedNomenclatureSubGroup = NomenclatureSubGroups[0];
        //        }
        //    }
        //    else
        //    {
        //        if (!NomenclatureSubGroups.Contains(SelectedNomenclatureSubGroup)) SelectedNomenclatureSubGroup = null;
        //    }
        //}

        private void CheckSelectedSelectedUnitStorage()
        {
            if (SelectedUnitStorage == null)
            {
                if (UnitStorages.Count != 0)
                {
                    SelectedUnitStorage = UnitStorages[0];
                }
            }
            else
            {
                if (!UnitStorages.Contains(SelectedUnitStorage)) SelectedUnitStorage = null;
            }
        }

        private void CheckSelectedSelectedWarrantyPeriod()
        {
            if (SelectedWarrantyPeriod == null)
            {
                SelectedWarrantyPeriod = WarrantyPeriods.FirstOrDefault();
            }
            else
            {
                if (!WarrantyPeriods.Contains(SelectedWarrantyPeriod)) SelectedWarrantyPeriod = null;
            }
        }

        public async void Update(AddProductViewModel.ChoiceUpdate choice)
        {
            switch (choice)
            {
                case AddProductViewModel.ChoiceUpdate.Store:
                    await StoreList.Load();
                    CheckSelectedStore();
                    break;
                case AddProductViewModel.ChoiceUpdate.NomenclatureGroup:
                    //if (_selectedStore == null) await NomenclatureGroupList.GetListNomenclatureGroup(-1);
                    //else await NomenclatureGroupList.GetListNomenclatureGroup(_selectedStore.Id);
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case AddProductViewModel.ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.Load(-1);
                    else await NomenclatureSubgroupList.Load(_selectedNomenclatureGroup.Id);
                   // CheckSelectedSelectedNomenclatureSubgroup();
                    break;
                case AddProductViewModel.ChoiceUpdate.UnitStorage:
                    await UnitStorageList.Load();
                    CheckSelectedSelectedUnitStorage();
                    break;
                case AddProductViewModel.ChoiceUpdate.WarrantyPeriod:
                    await WarrantyPeriodList.Load();
                    CheckSelectedSelectedWarrantyPeriod();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }

        public DelegateCommand AddProduct { get; }
    }
}

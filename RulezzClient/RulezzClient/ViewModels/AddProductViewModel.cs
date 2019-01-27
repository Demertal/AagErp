﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using RulezzClient.Model;
using RulezzClient.Properties;

namespace RulezzClient.ViewModels
{
    class AddProductViewModel : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            UnitStorage,
            WarrantyPeriod
        }

        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubgroup _selectedNomenclatureSubgroup;
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
        public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups => NomenclatureSubgroupList.NomenclatureSubgroups;
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages => UnitStorageList.UnitStorages;
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

        public AddProductViewModel()
        {
            Update(ChoiceUpdate.Store);
            Update(ChoiceUpdate.UnitStorage);
            Update(ChoiceUpdate.WarrantyPeriod);
            AddProduct = new DelegateCommand(() =>
            {
                if (Product.Add(Settings.Default.СconnectionString));
            });
        }

        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                Update(ChoiceUpdate.NomenclatureGroup);
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
                Update(ChoiceUpdate.NomenclatureSubgroup);
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public NomenclatureSubgroup SelectedNomenclatureSubgroup
        {
            get => _selectedNomenclatureSubgroup;
            set
            {
                _selectedNomenclatureSubgroup = value;
                if (_selectedNomenclatureSubgroup != null) Product.IdNomenclatureSubgroup = _selectedNomenclatureSubgroup.Id;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public UnitStorage SelectedUnitStorage
        {
            get => _selectedUnitStorage;
            set
            {
                _selectedUnitStorage = value;
                if (_selectedUnitStorage != null) Product.UnitStorage = _selectedUnitStorage.Id.ToString();
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
                if (_selectedWarrantyPeriod != null) Product.Warranty = _selectedWarrantyPeriod.Id.ToString();
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public bool IsButtonAddEnabled => SelectedStore != null && SelectedNomenclatureGroup != null && SelectedNomenclatureSubgroup != null && SelectedUnitStorage != null && SelectedWarrantyPeriod != null && Product.Title != null && Product.Title != "" && Product.Title != "" && Product.VendorCode.Length <= 20 && Product.Barcode.Length <= 13;

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
                case ChoiceUpdate.UnitStorage:
                    await UnitStorageList.GetListUnitStorage();
                    CheckSelectedSelectedUnitStorage();
                    break;
                case ChoiceUpdate.WarrantyPeriod:
                    await WarrantyPeriodList.GetListWarrantyPeriod();
                    CheckSelectedSelectedWarrantyPeriod();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
        }

        public DelegateCommand AddProduct { get; }
    }
}

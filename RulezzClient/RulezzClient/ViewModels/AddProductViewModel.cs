using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

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
        private NomenclatureSubGroup _selectedNomenclatureSubGroup;
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
        public ReadOnlyObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups => NomenclatureSubgroupList.NomenclatureSubGroups;
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages => UnitStorageList.UnitStorages;
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

        public AddProductViewModel()
        {
            Update(ChoiceUpdate.Store);
            Update(ChoiceUpdate.UnitStorage);
            Update(ChoiceUpdate.WarrantyPeriod);
            AddProduct = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    Product.IdExchangeRate = db.ExchangeRate.FirstOrDefault(r => r.Title == "грн").Id;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Product.Add(Product);
                            db.SaveChanges();
                            transaction.Commit();
                            MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
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

        public NomenclatureSubGroup SelectedNomenclatureSubGroup
        {
            get => _selectedNomenclatureSubGroup;
            set
            {
                _selectedNomenclatureSubGroup = value;
                if (_selectedNomenclatureSubGroup != null) Product.IdNomenclatureSubGroup = _selectedNomenclatureSubGroup.Id;
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
                if (_selectedUnitStorage != null) Product.IdUnitStorage = _selectedUnitStorage.Id;
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
                if (_selectedWarrantyPeriod != null) Product.IdWarrantyPeriod = _selectedWarrantyPeriod.Id;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public bool IsButtonAddEnabled => SelectedStore != null && SelectedNomenclatureGroup != null && SelectedNomenclatureSubGroup != null && SelectedUnitStorage != null && SelectedWarrantyPeriod != null && Product.Title != null && Product.Title != "";

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
                    await StoreList.Load();
                    CheckSelectedStore();
                    break;
                case ChoiceUpdate.NomenclatureGroup:
                    if (_selectedStore == null) await NomenclatureGroupList.Load(-1);
                    else await NomenclatureGroupList.Load(_selectedStore.Id);
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.Load(-1);
                    else await NomenclatureSubgroupList.Load(_selectedNomenclatureGroup.Id);
                    CheckSelectedSelectedNomenclatureSubgroup();
                    break;
                case ChoiceUpdate.UnitStorage:
                    await UnitStorageList.Load();
                    CheckSelectedSelectedUnitStorage();
                    break;
                case ChoiceUpdate.WarrantyPeriod:
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

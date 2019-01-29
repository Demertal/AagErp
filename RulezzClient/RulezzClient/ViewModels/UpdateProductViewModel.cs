using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
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

        private readonly ProductView_Result _oldProduct;

        public Store _oldStore { get; }
        public NomenclatureGroup _oldNomenclatureGroup { get; }
        public NomenclatureSubGroup _oldNomenclatureSubGroup { get; }
        public string OldTitle => _oldProduct.Title;
        public string OldBarcode => _oldProduct.Barcode;
        public string OldVendorCode => _oldProduct.VendorCode;
        public string OldUnitStorage => _oldProduct.UnitStorage;
        public string OldWarranty => _oldProduct.PeriodString;

        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubGroup _selectedNomenclatureSubGroup;
        private UnitStorage _selectedUnitStorage;
        private WarrantyPeriod _selectedWarrantyPeriod;

        private Store _tempStore;
        private NomenclatureGroup _tempNomenclatureGroup;
        private NomenclatureSubGroup _tempNomenclatureSubGroup;

        public StoreListVm StoreList = new StoreListVm();
        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubgroupListVm NomenclatureSubgroupList = new NomenclatureSubgroupListVm();
        public UnitStorageListVm UnitStorageList = new UnitStorageListVm();
        public WarrantyPeriodListVm WarrantyPeriodList = new WarrantyPeriodListVm();
        public Product Product;

        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups => NomenclatureSubgroupList.NomenclatureSubGroups;
        public ReadOnlyObservableCollection<UnitStorage> UnitStorages => UnitStorageList.UnitStorages;
        public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

        public UpdateProductViewModel(ProductView_Result product, NomenclatureSubGroup nomenclatureSubgroup, NomenclatureGroup nomenclatureGroup, Store store, Window wnd)
        {
            using (StoreEntities db = new StoreEntities())
            {
                Product = db.Product.Find(product.Id);
            }
            _oldProduct = product;
            _oldNomenclatureSubGroup = nomenclatureSubgroup;
            _oldNomenclatureGroup = nomenclatureGroup;
            _oldStore = store;
            _tempStore = store;
            _tempNomenclatureGroup = nomenclatureGroup;
            _tempNomenclatureSubGroup = nomenclatureSubgroup;
            Update(AddProductViewModel.ChoiceUpdate.Store);
            Update(AddProductViewModel.ChoiceUpdate.WarrantyPeriod);
            Update(AddProductViewModel.ChoiceUpdate.UnitStorage);
            AddProduct = new DelegateCommand(() =>
            {
                bool sucsuccess = false;
                using (StoreEntities db = new StoreEntities())
                {
                    Product old = db.Product.Find(_oldProduct.Id);
                    if (Product == old) return;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            old.Title = Product.Title;
                            old.Barcode = Product.Barcode;
                            old.VendorCode = Product.VendorCode;
                            old.IdUnitStorage = Product.IdUnitStorage;
                            old.IdWarrantyPeriod = Product.IdWarrantyPeriod;
                            old.IdExchangeRate = Product.IdExchangeRate;
                            old.IdNomenclatureSubGroup = Product.IdNomenclatureSubGroup;
                            db.Entry(old).State = EntityState.Modified;
                            db.SaveChanges();
                            transaction.Commit();
                            sucsuccess = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        if (!sucsuccess) return;
                        MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK);
                        wnd.Close();
                    }
                }
            });
        }

        public string OldStore => _oldStore.Title;

        public string OldNomenclatureGroup => _oldNomenclatureGroup.Title;

        public string OldNomenclatureSubGroup => _oldNomenclatureSubGroup.Title;

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

        private void CheckSelectedSelectedNomenclatureSubGroup()
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

        public async void Update(AddProductViewModel.ChoiceUpdate choice)
        {
            switch (choice)
            {
                case AddProductViewModel.ChoiceUpdate.Store:
                    await StoreList.Load();
                    if (_tempStore != null)
                    {
                        SelectedStore = _tempStore;
                        _tempStore = null;
                    }
                    CheckSelectedStore();
                    break;
                case AddProductViewModel.ChoiceUpdate.NomenclatureGroup:
                    if (_selectedStore == null) await NomenclatureGroupList.Load(-1);
                    else await NomenclatureGroupList.Load(_selectedStore.Id);
                    if (_tempNomenclatureGroup != null)
                    {
                        SelectedNomenclatureGroup = _tempNomenclatureGroup;
                        _tempNomenclatureGroup = null;
                    }
                    CheckSelectedSelectedNomenclatureGroup();
                    break;
                case AddProductViewModel.ChoiceUpdate.NomenclatureSubgroup:
                    if (_selectedNomenclatureGroup == null) await NomenclatureSubgroupList.Load(-1);
                    else await NomenclatureSubgroupList.Load(_selectedNomenclatureGroup.Id);
                    if (_tempNomenclatureSubGroup != null)
                    {
                        SelectedNomenclatureSubGroup = _tempNomenclatureSubGroup;
                        _tempNomenclatureSubGroup = null;
                    }
                    CheckSelectedSelectedNomenclatureSubGroup();
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

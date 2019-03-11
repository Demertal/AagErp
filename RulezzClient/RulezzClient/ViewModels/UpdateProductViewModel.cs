//using System;
//using System.Collections.ObjectModel;
//using System.Data.Entity;
//using System.Linq;
//using System.Windows;
//using Prism.Commands;
//using Prism.Mvvm;
//using RulezzClient.Models;

//namespace RulezzClient.ViewModels
//{
//    class UpdateProductViewModel : BindableBase
//    {
//        public enum ChoiceUpdate : byte
//        {
//            Store,
//            NomenclatureGroup,
//            NomenclatureSubgroup,
//            UnitStorage,
//            WarrantyPeriod
//        }

//        private UnitStorages _selectedUnitStorage;
//        private WarrantyPeriods _selectedWarrantyPeriod;

//        public UnitStorageListVm UnitStorageList = new UnitStorageListVm();
//        public WarrantyPeriodListVm WarrantyPeriodList = new WarrantyPeriodListVm();
//        public Products Product;

//        public ReadOnlyObservableCollection<UnitStorages> UnitStorages => UnitStorageList.UnitStorages;
//        public ReadOnlyObservableCollection<WarrantyPeriods> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

//        public UpdateProductViewModel(ProductView product, Window wnd)
//        {
//            using (StoreEntities db = new StoreEntities())
//            {
//                Product = db.Products.Find(product.Id);
//            }
//            Update(AddProductViewModel.ChoiceUpdate.WarrantyPeriod);
//            Update(AddProductViewModel.ChoiceUpdate.UnitStorage);
//            AddProduct = new DelegateCommand(() =>
//            {
//                bool sucsuccess = false;
//                using (StoreEntities db = new StoreEntities())
//                {
//                    //Products old = db.Products.Find(_oldProduct.Id);
//                    //if (Product == old) return;
//                    //using (var transaction = db.Database.BeginTransaction())
//                    //{
//                    //    try
//                    //    {
//                    //        old.Title = Product.Title;
//                    //        old.Barcode = Product.Barcode;
//                    //        old.VendorCode = Product.VendorCode;
//                    //        old.IdUnitStorage = Product.IdUnitStorage;
//                    //        old.IdWarrantyPeriod = Product.IdWarrantyPeriod;
//                    //        old.IdExchangeRate = Product.IdExchangeRate;
//                    //        db.Entry(old).State = EntityState.Modified;
//                    //        db.SaveChanges();
//                    //        transaction.Commit();
//                    //        sucsuccess = true;
//                    //    }
//                    //    catch (Exception ex)
//                    //    {
//                    //        transaction.Rollback();
//                    //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                    //            MessageBoxImage.Error);
//                    //    }
//                    //    if (!sucsuccess) return;
//                    //    MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK);
//                    //    wnd.Close();
//                    //}
//                }
//            });
//        }

//        public UnitStorages SelectedUnitStorage
//        {
//            get => _selectedUnitStorage;
//            set
//            {
//                _selectedUnitStorage = value;
//                if (_selectedUnitStorage != null) Product.IdUnitStorage = _selectedUnitStorage.Id;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        public WarrantyPeriods SelectedWarrantyPeriod
//        {
//            get => _selectedWarrantyPeriod;
//            set
//            {
//                _selectedWarrantyPeriod = value;
//                if (_selectedWarrantyPeriod != null) Product.IdWarrantyPeriod = _selectedWarrantyPeriod.Id;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        public bool IsButtonAddEnabled => SelectedUnitStorage != null && SelectedWarrantyPeriod != null && Product.Title != null && Product.Title != "";

//        public string Title
//        {
//            get => Product.Title;
//            set
//            {
//                Product.Title = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        public string Barcode
//        {
//            get => Product.Barcode;
//            set
//            {
//                Product.Barcode = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        public string VendorCode
//        {
//            get => Product.VendorCode;
//            set
//            {
//                Product.VendorCode = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        private void CheckSelectedSelectedUnitStorage()
//        {
//            if (SelectedUnitStorage == null)
//            {
//                if (UnitStorages.Count != 0)
//                {
//                    SelectedUnitStorage = UnitStorages[0];
//                }
//            }
//            else
//            {
//                if (!UnitStorages.Contains(SelectedUnitStorage)) SelectedUnitStorage = null;
//            }
//        }

//        private void CheckSelectedSelectedWarrantyPeriod()
//        {
//            if (SelectedWarrantyPeriod == null)
//            {
//                SelectedWarrantyPeriod = WarrantyPeriods.FirstOrDefault();
//            }
//            else
//            {
//                if (!WarrantyPeriods.Contains(SelectedWarrantyPeriod)) SelectedWarrantyPeriod = null;
//            }
//        }

//        public async void Update(AddProductViewModel.ChoiceUpdate choice)
//        {
//            switch (choice)
//            {
//               case AddProductViewModel.ChoiceUpdate.UnitStorage:
//                    await UnitStorageList.Load();
//                    CheckSelectedSelectedUnitStorage();
//                    break;
//                case AddProductViewModel.ChoiceUpdate.WarrantyPeriod:
//                    await WarrantyPeriodList.Load();
//                    CheckSelectedSelectedWarrantyPeriod();
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
//            }
//        }

//        public DelegateCommand AddProduct { get; }
//    }
//}

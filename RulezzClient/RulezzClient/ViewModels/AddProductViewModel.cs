//using System;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Windows;
//using Prism.Commands;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class AddProductViewModel : BindableBase
//    {
//        public enum ChoiceUpdate : byte
//        {
//            UnitStorage,
//            WarrantyPeriod
//        }

//        #region Parametrs

//        private UnitStorages _selectedUnitStorage;
//        private WarrantyPeriods _selectedWarrantyPeriod;
//        private ShowStructurVM _showStructur;

//        public UnitStorageListVm UnitStorageList = new UnitStorageListVm();
//        public WarrantyPeriodListVm WarrantyPeriodList = new WarrantyPeriodListVm();
//        public Products Product = new Products();

//        public ReadOnlyObservableCollection<UnitStorages> UnitStorages => UnitStorageList.UnitStorages;
//        public ReadOnlyObservableCollection<WarrantyPeriods> WarrantyPeriods => WarrantyPeriodList.WarrantyPeriods;

//        #endregion

//        public AddProductViewModel()
//        {
//            ShowStructur = new ShowStructurVM();
//            Update(ChoiceUpdate.UnitStorage);
//            Update(ChoiceUpdate.WarrantyPeriod);
//            using (StoreEntities db = new StoreEntities())
//            {
//                ExchangeRates exchange = db.ExchangeRates.FirstOrDefault(r => r.Title == "грн");
//                if (exchange != null) Product.IdExchangeRate = exchange.Id;
//                else throw new Exception("Нет старотовой валюты");
//            }

//            AddProduct = new DelegateCommand(() =>
//            {
//                using (StoreEntities db = new StoreEntities())
//                {
//                    using (var transaction = db.Database.BeginTransaction())
//                    {
//                        try
//                        {
//                            Product.IdGroup = ShowStructur.SelectedNode.Group.Id;
//                            db.Products.Add(Product);
//                            db.SaveChanges();
//                            transaction.Commit();
//                            Product.Id = 0;
//                            MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK);
//                        }
//                        catch (Exception ex)
//                        {
//                            transaction.Rollback();
//                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                                MessageBoxImage.Error);
//                        }
//                    }
//                }
//            });
//        }

//        #region GetSetMethod

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

//        public bool IsButtonAddEnabled => SelectedUnitStorage != null && SelectedWarrantyPeriod != null && Product.Title != null && Product.Title != "" && ShowStructur?.SelectedNode != null;

//        public ShowStructurVM ShowStructur
//        {
//            get => _showStructur;
//            set
//            {
//                _showStructur = value;
//                RaisePropertyChanged();
//            }
//        }

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

//        #endregion

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

//        public async void Update(ChoiceUpdate choice)
//        {
//            switch (choice)
//            {
//                case ChoiceUpdate.UnitStorage:
//                    await UnitStorageList.Load();
//                    CheckSelectedSelectedUnitStorage();
//                    break;
//                case ChoiceUpdate.WarrantyPeriod:
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

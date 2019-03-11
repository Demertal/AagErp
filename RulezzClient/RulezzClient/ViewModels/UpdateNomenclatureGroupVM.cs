//using System;
//using System.Collections.ObjectModel;
//using System.Data.Entity;
//using System.Windows;
//using Prism.Commands;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    class UpdateNomenclatureGroupVM : BindableBase
//    {
//        private Store _tempStore;
//        public Store _oldStore { get; }
//        public NomenclatureGroup _oldNomenclatureGroup { get; }

//        private Store _selectedStore;

//        public StoreListVm StoreList = new StoreListVm();
//        public NomenclatureGroup NomenclatureGroup;

//        public ReadOnlyObservableCollection<Store> Stores => StoreList.Stores;

//        public UpdateNomenclatureGroupVM(NomenclatureGroup nomenclatureGroup, Store store, Window wnd)
//        {
//            _tempStore = store;
//            _oldNomenclatureGroup = nomenclatureGroup;
//            _oldStore = store;
//            NomenclatureGroup = nomenclatureGroup;
//            Update();
//            AddNode = new DelegateCommand(() =>
//            {
//                bool sucsuccess = false;
//                using (StoreEntities db = new StoreEntities())
//                {
//                    NomenclatureGroup old = db.NomenclatureGroup.Find(NomenclatureGroup.Id);
//                    if (NomenclatureGroup == old) return;
//                    using (var transaction = db.Database.BeginTransaction())
//                    {
//                        try
//                        {
//                            old.Title = NomenclatureGroup.Title;
//                            old.IdStore = NomenclatureGroup.IdStore;
//                            db.Entry(old).State = EntityState.Modified;
//                            db.SaveChanges();
//                            transaction.Commit();
//                            sucsuccess = true;
//                        }
//                        catch (Exception ex)
//                        {
//                            transaction.Rollback();
//                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                                MessageBoxImage.Error);
//                        }
//                        if (!sucsuccess) return;
//                        MessageBox.Show("Номенклатурная группа изменена", "Успех", MessageBoxButton.OK);
//                        wnd.Close();
//                    }
//                }
//            });
//        }

//        public string OldStore => _oldStore.Title;

//        public string OldNomenclatureGroup => _oldNomenclatureGroup.Title;

//        public Store SelectedStore
//        {
//            get => _selectedStore;
//            set
//            {
//                _selectedStore = value;
//                NomenclatureGroup.IdStore = _selectedStore.Id;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        public bool IsButtonAddEnabled => SelectedStore != null && NomenclatureGroup.Title != null && NomenclatureGroup.Title != "";

//        public string Title
//        {
//            get => NomenclatureGroup.Title;
//            set
//            {
//                NomenclatureGroup.Title = value;
//                RaisePropertyChanged();
//                RaisePropertyChanged("IsButtonAddEnabled");
//            }
//        }

//        private void CheckSelectedStore()
//        {
//            if (SelectedStore == null)
//            {
//                if (Stores.Count != 0)
//                {
//                    SelectedStore = Stores[0];
//                }
//            }
//            else
//            {
//                if (!Stores.Contains(_selectedStore)) SelectedStore = null;
//            }
//        }

//        public async void Update()
//        {
//            await StoreList.Load();
//            if (_tempStore != null)
//            {
//                SelectedStore = _tempStore;
//                _tempStore = null;
//            }
//            CheckSelectedStore();
//        }

//        public DelegateCommand AddNode { get; }
//    }
//}
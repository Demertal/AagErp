using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ShowUnitStorageVM : BindableBase
    {
        private UnitStorage _selectedUnitStorage;

        private readonly IUiDialogService _dialogService = new DialogService();

        public UnitStorageListVm UnitStorageList = new UnitStorageListVm();

        public ReadOnlyObservableCollection<UnitStorage> UnitStorages => UnitStorageList.UnitStorages;

        public ShowUnitStorageVM()
        {
            UnitStorageList.Load();
            CellChanged = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var unt = db.UnitStorage.Find(_selectedUnitStorage.Id);
                            unt.Title = _selectedUnitStorage.Title;
                            db.Entry(unt).State = EntityState.Modified;
                            db.SaveChanges();
                            transaction.Commit();
                            UnitStorageList.Load();
                            RaisePropertyChanged("UnitStorages");
                            MessageBox.Show("Наименование ед. хранения измененено", "Успех", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            UnitStorageList.Load();
                            RaisePropertyChanged("UnitStorages");
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
            });
            DeleteUnitStorage = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var unt = db.UnitStorage.Find(_selectedUnitStorage.Id);
                            db.Entry(unt).State = EntityState.Deleted;
                            db.SaveChanges();
                            transaction.Commit();
                            UnitStorageList.Load();
                            RaisePropertyChanged("UnitStorages");
                            MessageBox.Show("Ед. хранения удалена", "Успех", MessageBoxButton.OK);
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
            AddUnitStorage = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog(DialogService.ChoiceView.AddUnitStorage, null, true, b => { });
                UnitStorageList.Load();
            });
        }

        public UnitStorage SelectedUnitStorage
        {
            get => _selectedUnitStorage;
            set
            {
                _selectedUnitStorage = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand DeleteUnitStorage { get; }

        public DelegateCommand AddUnitStorage { get; }

        public DelegateCommand CellChanged { get; }
    }
}

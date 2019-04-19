using System.Collections.ObjectModel;
using ModelModul.UnitStorage;
using Prism.Commands;
using Prism.Mvvm;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStorageViewModel : BindableBase
    {
        //private UnitStorages _selectedUnitStorage;

        public DbSetUnitStoragesModel DbSetUnitStoragesModel = new DbSetUnitStoragesModel();

        public ObservableCollection<UnitStorageModel> UnitStorages => DbSetUnitStoragesModel.UnitStorages;

        public ShowUnitStorageViewModel()
        {
            DbSetUnitStoragesModel.Load();
            CellChanged = new DelegateCommand(() =>
            {
                //using (StoreEntities db = new StoreEntities())
                //{
                //    using (var transaction = db.Database.BeginTransaction())
                //    {
                //        try
                //        {
                //            var unt = db.UnitStorages.Find(_selectedUnitStorage.Id);
                //            unt.Title = _selectedUnitStorage.Title;
                //            db.Entry(unt).State = EntityState.Modified;
                //            db.SaveChanges();
                //            transaction.Commit();
                //            DbSetGroupsModel.Load();
                //            RaisePropertyChanged("UnitStorages");
                //            MessageBox.Show("Наименование ед. хранения измененено", "Успех", MessageBoxButton.OK);
                //        }
                //        catch (Exception ex)
                //        {
                //            transaction.Rollback();
                //            DbSetGroupsModel.Load();
                //            RaisePropertyChanged("UnitStorages");
                //            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                //                MessageBoxImage.Error);
                //        }
                //    }
                //}
            });
            DeleteUnitStorage = new DelegateCommand(() =>
            {
                //using (StoreEntities db = new StoreEntities())
                //{
                //    using (var transaction = db.Database.BeginTransaction())
                //    {
                //        try
                //        {
                //            var unt = db.UnitStorages.Find(_selectedUnitStorage.Id);
                //            db.Entry(unt).State = EntityState.Deleted;
                //            db.SaveChanges();
                //            transaction.Commit();
                //            DbSetGroupsModel.Load();
                //            RaisePropertyChanged("UnitStorages");
                //            MessageBox.Show("Ед. хранения удалена", "Успех", MessageBoxButton.OK);
                //        }
                //        catch (Exception ex)
                //        {
                //            transaction.Rollback();
                //            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                //                MessageBoxImage.Error);
                //        }
                //    }
                //}
            });
            AddUnitStorage = new DelegateCommand(() =>
            {
                //_dialogService.ShowDialog(DialogService.ChoiceView.AddUnitStorage, null, true, b => { });
                //DbSetGroupsModel.Load();
            });
        }

        public UnitStorageModel SelectedUnitStorage
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

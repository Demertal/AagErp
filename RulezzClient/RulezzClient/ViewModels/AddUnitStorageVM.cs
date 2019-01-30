using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class AddUnitStorageVM : BindableBase
    {
        private string _title = "";

        public AddUnitStorageVM(Window wnd)
        {
            AddUnitStorage = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        UnitStorage st = new UnitStorage
                        {
                            Title = Title
                        };
                        try
                        {
                            db.UnitStorage.Add(st);
                            db.SaveChanges();
                            transaction.Commit();
                            MessageBox.Show("Ед. хранеия добавлена", "Успех", MessageBoxButton.OK);
                            wnd.Close();
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

        public bool IsButtonAddEnabled => Title != "";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public DelegateCommand AddUnitStorage { get; }
    }
}

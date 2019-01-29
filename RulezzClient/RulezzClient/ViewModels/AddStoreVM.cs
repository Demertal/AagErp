using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class AddStoreVM : BindableBase
    {
        private string _title = "";

        public AddStoreVM(Window wnd)
        {
            AddStore = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        Store st = new Store
                        {
                            Title = Title
                        };
                        try
                        {
                            db.Store.Add(st);
                            db.SaveChanges();
                            transaction.Commit();
                            MessageBox.Show("Магазин добавлен", "Успех", MessageBoxButton.OK);
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

        public DelegateCommand AddStore { get; }
    }
}

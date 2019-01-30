using System;
using System.Data.Entity;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class UpdateStoreVM : BindableBase
    {
        public Store _oldStore { get; }

        private string _title;

        public UpdateStoreVM(Store store, Window wnd)
        {
            _oldStore = store;
            Title = store.Title;
            AddStore = new DelegateCommand(() =>
            {
                bool sucsuccess = false;
                using (StoreEntities db = new StoreEntities())
                {
                    Store old = db.Store.Find(_oldStore.Id);
                    if (Title == old.Title) return;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            old.Title = Title;
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
                        MessageBox.Show("Магазин изменен", "Успех", MessageBoxButton.OK);
                        wnd.Close();
                    }
                }
            });
        }

        public string OldStore => _oldStore.Title;
        
        public bool IsButtonAddEnabled => !string.IsNullOrEmpty(Title);

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
using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class AddNomenclatureGroupVM : BindableBase
    {
        private Visibility _addVisibility;
        private string _title = "";

        public AddNomenclatureGroupVM(Store st, Window wnd)
        {
            AddVisibility = Visibility.Collapsed;
            AddNomenclatureGroup = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        NomenclatureGroup ng = new NomenclatureGroup
                        {
                            Title = Title,
                            IdStore = st.Id
                        };
                        try
                        {
                            db.NomenclatureGroup.Add(ng);
                            db.SaveChanges();
                            transaction.Commit();
                            MessageBox.Show("Номенклатурная группа добавлена", "Успех", MessageBoxButton.OK);
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

        public Visibility AddVisibility
        {
            get => _addVisibility;
            set
            {
                _addVisibility = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand AddNomenclatureGroup { get; }
    }
}

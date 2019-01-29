using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class AddNomenclatureSubGroupVM : BindableBase
    {
        private Visibility _addVisibility;
        private string _title = "";

        public AddNomenclatureSubGroupVM(NomenclatureGroup ng, Window wnd)
        {
            AddVisibility = Visibility.Collapsed;
            AddNomenclatureSubGroup = new DelegateCommand(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        NomenclatureSubGroup nsg = new NomenclatureSubGroup
                        {
                            Title = Title,
                            IdNomenclatureGroup = ng.Id,
                            IdPriceGroup = 1
                        };
                        try
                        {
                            db.NomenclatureSubGroup.Add(nsg);
                            db.SaveChanges();
                            transaction.Commit();
                            MessageBox.Show("Номенклатурная подгруппа добавлена", "Успех", MessageBoxButton.OK);
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

        public DelegateCommand AddNomenclatureSubGroup { get; }
    }
}
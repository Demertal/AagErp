using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class UpdateNomenclatureSubGroupVM : BindableBase
    {
        private NomenclatureGroup _tempNomenclatureGroup;
        private NomenclatureGroup _oldNomenclatureGroup { get; }
        private NomenclatureSubGroup _oldNomenclatureSubGroup { get; }

        private NomenclatureGroup _selectedNomenclatureGroup;

        public NomenclatureGroupListVm NomenclatureGroupList = new NomenclatureGroupListVm();
        public NomenclatureSubGroup NomenclatureSubGroup;

        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => NomenclatureGroupList.NomenclatureGroups;

        public UpdateNomenclatureSubGroupVM(NomenclatureSubGroup nomenclatureSubGroup, NomenclatureGroup nomenclatureGroup, Window wnd)
        {
            _tempNomenclatureGroup = nomenclatureGroup;
            _oldNomenclatureSubGroup = nomenclatureSubGroup;
            _oldNomenclatureGroup = nomenclatureGroup;
            NomenclatureSubGroup = nomenclatureSubGroup;
            Update();
            AddNomenclatureSubGroup = new DelegateCommand(() =>
            {
                bool sucsuccess = false;
                using (StoreEntities db = new StoreEntities())
                {
                    NomenclatureSubGroup old = db.NomenclatureSubGroup.Find(NomenclatureSubGroup.Id);
                    if (NomenclatureSubGroup == old) return;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            old.Title = NomenclatureSubGroup.Title;
                            old.IdNomenclatureGroup = NomenclatureSubGroup.IdNomenclatureGroup;
                            old.IdPriceGroup = NomenclatureSubGroup.IdPriceGroup;
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
                        MessageBox.Show("Номенклатурная подгруппа изменена", "Успех", MessageBoxButton.OK);
                        wnd.Close();
                    }
                }
            });
        }

        public string OldNomenclatureGroup => _oldNomenclatureGroup.Title;

        public string OldNomenclatureSubGroup => _oldNomenclatureSubGroup.Title;

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                NomenclatureSubGroup.IdNomenclatureGroup = _selectedNomenclatureGroup.Id;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        public bool IsButtonAddEnabled => SelectedNomenclatureGroup != null && NomenclatureSubGroup.Title != null && NomenclatureSubGroup.Title != "";

        public string Title
        {
            get => NomenclatureSubGroup.Title;
            set
            {
                NomenclatureSubGroup.Title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsButtonAddEnabled");
            }
        }

        private void CheckSelectedNomenclatureGroup()
        {
            if (SelectedNomenclatureGroup == null)
            {
                if (NomenclatureGroups.Count != 0)
                {
                    SelectedNomenclatureGroup = NomenclatureGroups[0];
                }
            }
            else
            {
                if (!NomenclatureGroups.Contains(_selectedNomenclatureGroup)) SelectedNomenclatureGroup = null;
            }
        }

        public async void Update()
        {
            await NomenclatureGroupList.Load(_tempNomenclatureGroup.IdStore);
            if (_tempNomenclatureGroup != null)
            {
                SelectedNomenclatureGroup = _tempNomenclatureGroup;
                _tempNomenclatureGroup = null;
            }
            CheckSelectedNomenclatureGroup();
        }

        public DelegateCommand AddNomenclatureSubGroup { get; }
    }
}
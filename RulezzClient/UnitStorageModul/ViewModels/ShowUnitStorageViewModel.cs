using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul;
using ModelModul.UnitStorage;
using Prism.Commands;
using Prism.Mvvm;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStorageViewModel : BindableBase
    {
        #region Properties

        private readonly DbSetUnitStorages _dbSetUnitStorages = new DbSetUnitStorages();

        public ObservableCollection<UnitStorages> UnitStoragesList => _dbSetUnitStorages.List;

        public DelegateCommand<object> AddUnitStoragesCommand { get; }

        public DelegateCommand<UnitStorages> DeleteUnitStoragesCommand { get; }

        #endregion

        public ShowUnitStorageViewModel()
        {
            Load();
            AddUnitStoragesCommand = new DelegateCommand<object>(AddUnitStorages);
            DeleteUnitStoragesCommand = new DelegateCommand<UnitStorages>(DeleteUnitStorages);
        }

        private void DeleteUnitStorages(UnitStorages obj)
        {
            if (obj == null) return;
            if (obj.Title == "шт")
            {
                MessageBox.Show("Нельзя удалять ед. хр.: \"шт\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Удалить ед. хр.?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                _dbSetUnitStorages.Delete(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void AddUnitStorages(object obj)
        {
            try
            {
                if (obj == null) return;
                if (((UnitStorages)obj).Id == 0)
                {
                    _dbSetUnitStorages.Add((UnitStorages)obj);
                }
                else
                {
                    _dbSetUnitStorages.Update((UnitStorages)obj);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void Load()
        {
            try
            {
                await _dbSetUnitStorages.Load();
                RaisePropertyChanged("UnitStoragesList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

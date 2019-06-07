using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.UnitStorage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStorageViewModel : BindableBase, INavigationAware
    {
        #region Properties

        public ObservableCollection<UnitStorages> _unitStoragesList = new ObservableCollection<UnitStorages>();
        public ObservableCollection<UnitStorages> UnitStoragesList
        {
            get => _unitStoragesList;
            set => SetProperty(ref _unitStoragesList, value);
        }


        public DelegateCommand<UnitStorages> DeleteUnitStoragesCommand { get; }
        public DelegateCommand<DataGridRowEditEndingEventArgs> ChangeUnitStoragesCommand { get; }

        #endregion

        public ShowUnitStorageViewModel()
        {
            Load();
            ChangeUnitStoragesCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangeUnitStorages);
            DeleteUnitStoragesCommand = new DelegateCommand<UnitStorages>(DeleteUnitStorages);
        }

        private async void ChangeUnitStorages(DataGridRowEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((UnitStorages)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddUnitStorages((UnitStorages)obj.Row.Item);
                }
                else
                {
                    UpdateUnitStorages((UnitStorages)obj.Row.Item);
                }
            }
        }

        private async void AddUnitStorages(UnitStorages obj)
        {
            try
            {
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                await dbSet.AddAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void UpdateUnitStorages(UnitStorages obj)
        {
            try
            {
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                await dbSet.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void DeleteUnitStorages(UnitStorages obj)
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
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                await dbSet.DeleteAsync(obj.Id);
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
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                UnitStoragesList = new ObservableCollection<UnitStorages>(await dbSet.LoadAsync());
                RaisePropertyChanged("UnitStoragesList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.UnitStorage;
using Prism.Commands;
using Prism.Regions;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStorageViewModel : ViewModelBase
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
            ChangeUnitStoragesCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangeUnitStorages);
            DeleteUnitStoragesCommand = new DelegateCommand<UnitStorages>(DeleteUnitStorages);
        }

        private void ChangeUnitStorages(DataGridRowEditEndingEventArgs obj)
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

        private void AddUnitStorages(UnitStorages obj)
        {
            try
            {
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                dbSet.Add(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void UpdateUnitStorages(UnitStorages obj)
        {
            try
            {
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                dbSet.Update(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
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
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                dbSet.Delete(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void Load()
        {
            try
            {
                DbSetUnitStorages dbSet = new DbSetUnitStorages();
                UnitStoragesList = new ObservableCollection<UnitStorages>(dbSet.Load());
                RaisePropertyChanged("UnitStoragesList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}

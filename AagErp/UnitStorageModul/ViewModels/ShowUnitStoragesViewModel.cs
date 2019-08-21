using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStoragesViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<UnitStorage> _unitStoragesList = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStoragesList;
            set => SetProperty(ref _unitStoragesList, value);
        }


        public DelegateCommand<UnitStorage> DeleteUnitStoragesCommand { get; }
        public DelegateCommand<DataGridRowEditEndingEventArgs> ChangeUnitStoragesCommand { get; }

        #endregion

        public ShowUnitStoragesViewModel()
        {
            ChangeUnitStoragesCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangeUnitStorages);
            DeleteUnitStoragesCommand = new DelegateCommand<UnitStorage>(DeleteUnitStoragesAsync);
        }

        private void ChangeUnitStorages(DataGridRowEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((UnitStorage)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddUnitStoragesAsync((UnitStorage)obj.Row.Item);
                }
                else
                {
                    UpdateUnitStoragesAsync((UnitStorage)obj.Row.Item);
                }
            }
        }

        private async void AddUnitStoragesAsync(UnitStorage obj)
        {
            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                await unitStorageRepository.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void UpdateUnitStoragesAsync(UnitStorage obj)
        {
            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                await unitStorageRepository.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void DeleteUnitStoragesAsync(UnitStorage obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить ед. хр.?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                await unitStorageRepository.DeleteAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void LoadAsync()
        {
            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                UnitStoragesList = new ObservableCollection<UnitStorage>(await unitStorageRepository.GetListAsync());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadAsync();
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

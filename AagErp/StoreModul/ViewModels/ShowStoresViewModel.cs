using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace StoreModul.ViewModels
{
    class ShowStoresViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Store> _storesList = new ObservableCollection<Store>();
        public ObservableCollection<Store> StoresList
        {
            get => _storesList;
            set => SetProperty(ref _storesList, value);
        }


        public DelegateCommand<Store> DeleteStoresCommand { get; }
        public DelegateCommand<DataGridRowEditEndingEventArgs> ChangeStoresCommand { get; }

        #endregion

        public ShowStoresViewModel()
        {
            ChangeStoresCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangeStores);
            DeleteStoresCommand = new DelegateCommand<Store>(DeleteStoresAsync);
        }

        private void ChangeStores(DataGridRowEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((Store)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddStoresAsync((Store)obj.Row.Item);
                }
                else
                {
                    UpdateStoresAsync((Store)obj.Row.Item);
                }
            }
        }

        private async void AddStoresAsync(Store obj)
        {
            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();
                await storeRepository.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void UpdateStoresAsync(Store obj)
        {
            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();
                await storeRepository.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void DeleteStoresAsync(Store obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить склад?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();
                await storeRepository.DeleteAsync(obj);
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
                IRepository<Store> storeRepository = new SqlStoreRepository();
                StoresList = new ObservableCollection<Store>(await storeRepository.GetListAsync());
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

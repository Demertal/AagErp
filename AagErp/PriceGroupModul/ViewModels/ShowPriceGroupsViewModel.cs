using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace PriceGroupModul.ViewModels
{
    class ShowPriceGroupsViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<PriceGroup> _priceGroupsList = new ObservableCollection<PriceGroup>();
        public ObservableCollection<PriceGroup> PriceGroupsList
        {
            get => _priceGroupsList;
            set => SetProperty(ref _priceGroupsList, value);
        }


        public DelegateCommand<PriceGroup> DeletePriceGroupsCommand { get; }
        public DelegateCommand<DataGridRowEditEndingEventArgs> ChangePriceGroupsCommand { get; }

        #endregion

        public ShowPriceGroupsViewModel()
        {
            ChangePriceGroupsCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangePriceGroups);
            DeletePriceGroupsCommand = new DelegateCommand<PriceGroup>(DeletePriceGroupsAsync);
        }

        private void ChangePriceGroups(DataGridRowEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (((PriceGroup)obj.Row.DataContext).Markup <= 0)
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddPriceGroupsAsync((PriceGroup)obj.Row.Item);
                }
                else
                {
                    UpdatePriceGroupsAsync((PriceGroup)obj.Row.Item);
                }
            }
        }

        private async void AddPriceGroupsAsync(PriceGroup obj)
        {
            try
            {
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                await priceGroupRepository.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void UpdatePriceGroupsAsync(PriceGroup obj)
        {
            try
            {
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                await priceGroupRepository.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void DeletePriceGroupsAsync(PriceGroup obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить наценку?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                await priceGroupRepository.DeleteAsync(obj);
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
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                PriceGroupsList = new ObservableCollection<PriceGroup>(await priceGroupRepository.GetListAsync());
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

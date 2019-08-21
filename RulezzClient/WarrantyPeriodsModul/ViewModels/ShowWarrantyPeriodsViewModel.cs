using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace WarrantyPeriodsModul.ViewModels
{
    class ShowWarrantyPeriodsViewModel: ViewModelBase
    {
        #region Properties

        public ObservableCollection<WarrantyPeriod> _warrantyPeriodsList = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriodsList;
            set => SetProperty(ref _warrantyPeriodsList, value);
        }

        public DelegateCommand<WarrantyPeriod> DeleteWarrantyPeriodsCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangeWarrantyPeriodsCommand { get; }

        #endregion

        public ShowWarrantyPeriodsViewModel()
        {
            ChangeWarrantyPeriodsCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangeWarrantyPeriods);
            DeleteWarrantyPeriodsCommand = new DelegateCommand<WarrantyPeriod>(DeleteWarrantyPeriodsAsync);
        }

        private void ChangeWarrantyPeriods(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((WarrantyPeriod)obj.Row.DataContext).Period))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddWarrantyPeriodsAsync((WarrantyPeriod)obj.Row.Item);
                }
                else
                {
                    UpdateWarrantyPeriodsAsync((WarrantyPeriod)obj.Row.Item);
                }
            }
        }

        private async void AddWarrantyPeriodsAsync(WarrantyPeriod obj)
        {
            try
            {
                SqlWarrantyPeriodRepository sql = new SqlWarrantyPeriodRepository();
                await sql.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void UpdateWarrantyPeriodsAsync(WarrantyPeriod obj)
        {
            try
            {
                SqlWarrantyPeriodRepository sql = new SqlWarrantyPeriodRepository();
                await sql.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void DeleteWarrantyPeriodsAsync(WarrantyPeriod obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить гарантийный период?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlWarrantyPeriodRepository sql = new SqlWarrantyPeriodRepository();
                await sql.DeleteAsync(obj);
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
                IRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(await sqlWarrantyPeriodRepository.GetListAsync());
                RaisePropertyChanged("WarrantyPeriodsList");
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

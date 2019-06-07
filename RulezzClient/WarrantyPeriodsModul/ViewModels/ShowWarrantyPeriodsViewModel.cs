using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Mvvm;

namespace WarrantyPeriodsModul.ViewModels
{
    class ShowWarrantyPeriodsViewModel: BindableBase
    {
        #region Properties

        public ObservableCollection<WarrantyPeriods> _warrantyPeriodsList = new ObservableCollection<WarrantyPeriods>();
        public ObservableCollection<WarrantyPeriods> WarrantyPeriodsList
        {
            get => _warrantyPeriodsList;
            set => SetProperty(ref _warrantyPeriodsList, value);
        }

        public DelegateCommand<WarrantyPeriods> DeleteWarrantyPeriodsCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangeWarrantyPeriodsCommand { get; }

        #endregion

        public ShowWarrantyPeriodsViewModel()
        {
            Load();
            ChangeWarrantyPeriodsCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangeWarrantyPeriods);
            DeleteWarrantyPeriodsCommand = new DelegateCommand<WarrantyPeriods>(DeleteWarrantyPeriods);
        }

        private void ChangeWarrantyPeriods(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((WarrantyPeriods)obj.Row.DataContext).Period))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddWarrantyPeriods((WarrantyPeriods)obj.Row.Item);
                }
                else
                {
                    UpdateWarrantyPeriods((WarrantyPeriods)obj.Row.Item);
                }
            }
        }

        private async void AddWarrantyPeriods(WarrantyPeriods obj)
        {
            try
            {
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                await dbSet.AddAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void UpdateWarrantyPeriods(WarrantyPeriods obj)
        {
            try
            {
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                await dbSet.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void DeleteWarrantyPeriods(WarrantyPeriods obj)
        {
            if (obj == null) return;
            if (obj.Period == "Нет")
            {
                MessageBox.Show("Нельзя удалять гарантийный период: \"Нет\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Удалить гарантийный период?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
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
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriods>(await dbSet.LoadAsync());
                RaisePropertyChanged("WarrantyPeriodsList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Regions;

namespace WarrantyPeriodsModul.ViewModels
{
    class ShowWarrantyPeriodsViewModel: ViewModelBase
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

        private void AddWarrantyPeriods(WarrantyPeriods obj)
        {
            try
            {
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                dbSet.Add(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void UpdateWarrantyPeriods(WarrantyPeriods obj)
        {
            try
            {
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                dbSet.Update(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void DeleteWarrantyPeriods(WarrantyPeriods obj)
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
                DbSetWarrantyPeriods dbSet = new DbSetWarrantyPeriods();
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriods>(dbSet.Load());
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

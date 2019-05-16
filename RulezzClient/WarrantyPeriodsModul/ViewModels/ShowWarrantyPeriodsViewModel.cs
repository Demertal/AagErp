using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Mvvm;

namespace WarrantyPeriodsModul.ViewModels
{
    class ShowWarrantyPeriodsViewModel: BindableBase
    {
        #region Properties

        private readonly DbSetWarrantyPeriods _dbSetWarrantyPeriods = new DbSetWarrantyPeriods();

        public ObservableCollection<WarrantyPeriods> WarrantyPeriodsList => _dbSetWarrantyPeriods.List;

        public DelegateCommand<object> AddWarrantyPeriodsCommand { get; }

        public DelegateCommand<WarrantyPeriods> DeleteWarrantyPeriodsCommand { get; }

        #endregion

        public ShowWarrantyPeriodsViewModel()
        {
            Load();
            AddWarrantyPeriodsCommand = new DelegateCommand<object>(AddWarrantyPeriods);
            DeleteWarrantyPeriodsCommand = new DelegateCommand<WarrantyPeriods>(DeleteWarrantyPeriods);
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
                await _dbSetWarrantyPeriods.DeleteAsync(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private async void AddWarrantyPeriods(object obj)
        {
            try
            {
                if (obj == null) return;
                if (((WarrantyPeriods)obj).Id == 0)
                {
                    await _dbSetWarrantyPeriods.AddAsync((WarrantyPeriods)obj);
                }
                else
                {
                    await _dbSetWarrantyPeriods.UpdateAsync((WarrantyPeriods)obj);
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
                await _dbSetWarrantyPeriods.LoadAsync();
                RaisePropertyChanged("WarrantyPeriodsList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

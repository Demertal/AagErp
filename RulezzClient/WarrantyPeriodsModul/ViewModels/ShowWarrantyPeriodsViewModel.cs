using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                _dbSetWarrantyPeriods.Delete(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void AddWarrantyPeriods(object obj)
        {
            try
            {
                if (obj == null) return;
                if (((WarrantyPeriods)obj).Id == 0)
                {
                    _dbSetWarrantyPeriods.Add((WarrantyPeriods)obj);
                }
                else
                {
                    _dbSetWarrantyPeriods.Update((WarrantyPeriods)obj);
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
                await _dbSetWarrantyPeriods.Load();
                RaisePropertyChanged("WarrantyPeriodsList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

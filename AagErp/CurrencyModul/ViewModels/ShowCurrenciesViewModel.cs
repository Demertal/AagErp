using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace CurrencyModul.ViewModels
{
    class ShowCurrenciesViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Currency> _currenciesList = new ObservableCollection<Currency>();
        public ObservableCollection<Currency> CurrenciesList
        {
            get => _currenciesList;
            set => SetProperty(ref _currenciesList, value);
        }


        public DelegateCommand<Currency> DeleteCurrenciesCommand { get; }
        public DelegateCommand<DataGridRowEditEndingEventArgs> ChangeCurrenciesCommand { get; }

        #endregion

        public ShowCurrenciesViewModel()
        {
            ChangeCurrenciesCommand = new DelegateCommand<DataGridRowEditEndingEventArgs>(ChangeCurrencies);
            DeleteCurrenciesCommand = new DelegateCommand<Currency>(DeleteCurrenciesAsync);
        }

        private void ChangeCurrencies(DataGridRowEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((Currency)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddCurrenciesAsync((Currency)obj.Row.Item);
                }
                else
                {
                    UpdateCurrenciesAsync((Currency)obj.Row.Item);
                }
            }
        }

        private async void AddCurrenciesAsync(Currency obj)
        {
            try
            {
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                await currencyRepository.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void UpdateCurrenciesAsync(Currency obj)
        {
            try
            {
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                await currencyRepository.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadAsync();
        }

        private async void DeleteCurrenciesAsync(Currency obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить валюту?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                await currencyRepository.DeleteAsync(obj);
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
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                CurrenciesList = new ObservableCollection<Currency>(await currencyRepository.GetListAsync());
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

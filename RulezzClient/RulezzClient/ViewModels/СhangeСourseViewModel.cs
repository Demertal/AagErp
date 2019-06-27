using System;
using System.Windows;
using ModelModul;
using ModelModul.ExchangeRate;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace RulezzClient.ViewModels
{
    class СhangeСourseViewModel: ViewModelBase, IInteractionRequestAware
    {
        #region Properties

        private ExchangeRates _exchangeRates = new ExchangeRates();

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                Load();
            }
        }

        public decimal Course
        {
            get => _exchangeRates.Course;
            set
            {
                _exchangeRates.Course = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand СhangeСourseCommand { get; }

        public bool IsEnabled => Course > 0;

        #endregion

        public СhangeСourseViewModel()
        {
            СhangeСourseCommand = new DelegateCommand(СhangeСourse).ObservesCanExecute(() => IsEnabled);
        }

        public void СhangeСourse()
        {
            try
            {
                DbSetExchangeRates dbSetExchange = new DbSetExchangeRates();
                dbSetExchange.Update(_exchangeRates);
                MessageBox.Show("Курс изменен.", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Load()
        {
            try
            {
                DbSetExchangeRates dbSetExchange = new DbSetExchangeRates();
                _exchangeRates = dbSetExchange.Load("USD");
                RaisePropertyChanged("Course");
                RaisePropertyChanged("IsEnabled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
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

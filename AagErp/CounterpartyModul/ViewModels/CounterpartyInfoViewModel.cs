using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace CounterpartyModul.ViewModels
{
    class CounterpartyInfoViewModel: ViewModelBase
    {
        #region Properties

        private bool _isUpdate;

        public bool IsUpdate
        {
            get => _isUpdate;
            set => SetProperty(ref _isUpdate, value);
        }

        private Counterparty _oldCounterparty = new Counterparty();

        private Counterparty _selectedCounterparty = new Counterparty();
        public Counterparty SelectedCounterparty
        {
            get => _selectedCounterparty;
            set
            {
                LoadAsync();
                IsUpdate = false;
                SetProperty(ref _selectedCounterparty, value);
                _selectedCounterparty.PropertyChanged += (o, e) => RaisePropertyChanged(e.PropertyName);
                RaisePropertyChanged("CanUpdate");
            }
        }

        private ObservableCollection<PaymentType> _paymentTypesList = new ObservableCollection<PaymentType>();
        public ObservableCollection<PaymentType> PaymentTypesList
        {
            get => _paymentTypesList;
            set => SetProperty(ref _paymentTypesList, value);
        }

        public bool CanUpdate => SelectedCounterparty != null && SelectedCounterparty.Id != 0;

        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public CounterpartyInfoViewModel()
        {
            UpdateCommand = new DelegateCommand(Update).ObservesCanExecute(() => SelectedCounterparty.IsValidate);
            ResetCommand = new DelegateCommand(Reset);
            OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedCounterparty.IsValidate);
        }

        private void Accept()
        {
            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                counterpartyRepository.UpdateAsync(SelectedCounterparty);
                MessageBox.Show("Контрагент изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                IsUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset()
        {
            SelectedCounterparty.Title = _oldCounterparty.Title;
            SelectedCounterparty.Address = _oldCounterparty.Address;
            SelectedCounterparty.ContactPerson = _oldCounterparty.ContactPerson;
            SelectedCounterparty.ContactPhone = _oldCounterparty.ContactPhone;
            SelectedCounterparty.Props = _oldCounterparty.Props;
            SelectedCounterparty.IdPaymentType = _oldCounterparty.IdPaymentType;
            IsUpdate = false;
        }

        private void Update()
        {
            _oldCounterparty = (Counterparty)SelectedCounterparty.Clone();
            IsUpdate = true;
        }

        public async void LoadAsync()
        {
            IRepository<PaymentType> paymentTypeRepository = new SqlPaymentTypeRepository();
            PaymentTypesList = new ObservableCollection<PaymentType>(await paymentTypeRepository.GetListAsync());
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

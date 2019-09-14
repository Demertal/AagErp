using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;

namespace CounterpartyModul.ViewModels
{
    class CounterpartyInfoViewModel: ViewModelBase, IEditableObject
    {
        #region Properties

        private Counterparty _counterparty = new Counterparty();
        public Counterparty Counterparty
        {
            get => _counterparty;
            set
            {
                SetProperty(ref _counterparty, value);
                _counterparty.PropertyChanged += (o, e) => { RaisePropertyChanged(e.PropertyName); };
                RaisePropertyChanged("CanEdit");
            }
        }

        private ObservableCollection<PaymentType> _paymentTypesList = new ObservableCollection<PaymentType>();
        public ObservableCollection<PaymentType> PaymentTypesList
        {
            get => _paymentTypesList;
            set => SetProperty(ref _paymentTypesList, value);
        }

        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public CounterpartyInfoViewModel()
        {
            LoadAsync();
            UpdateCommand = new DelegateCommand(BeginEdit).ObservesCanExecute(() => CanEdit);
            ResetCommand = new DelegateCommand(CancelEdit);
            OkCommand = new DelegateCommand(EndEdit).ObservesCanExecute(() => Counterparty.IsValidate);
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

        #region EditableObject

        private Counterparty _backup = new Counterparty();

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public bool CanEdit => Counterparty != null && Counterparty.Id != 0;

        public void BeginEdit()
        {
            _backup = (Counterparty)Counterparty?.Clone();
            IsEdit = true;
            RaisePropertyChanged("Counterparty");
        }

        public async void EndEdit()
        {
            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                await counterpartyRepository.UpdateAsync(Counterparty);
                MessageBox.Show("Контрагент изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                IsEdit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CancelEdit()
        {
            Counterparty.Title = _backup.Title;
            Counterparty.Address = _backup.Address;
            Counterparty.ContactPerson = _backup.ContactPerson;
            Counterparty.ContactPhone = _backup.ContactPhone;
            Counterparty.Props = _backup.Props;
            Counterparty.IdPaymentType = _backup.IdPaymentType;
            IsEdit = false;
        }

        #endregion
    }
}

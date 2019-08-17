using System;
using System.ComponentModel;
using System.Windows;
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

        //private CounterpartyViewModel _selectedCounterparty = new CounterpartyViewModel();
        //public CounterpartyViewModel SelectedCounterparty
        //{
        //    get => _selectedCounterparty;
        //    set
        //    {
        //        _selectedCounterparty = value;
        //        IsUpdate = false;
        //        RaisePropertyChanged();
        //    }
        //}

        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public CounterpartyInfoViewModel()
        {
            //SelectedCounterparty.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            //{
            //    RaisePropertyChanged(args.PropertyName);
            //};
            //UpdateCommand = new DelegateCommand(Update).ObservesCanExecute(() => SelectedCounterparty.IsValidate);
            //ResetCommand = new DelegateCommand(Reset);
            //OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedCounterparty.IsValidate);
        }

        private void Accept()
        {
            try
            {
                SqlCounterpartyRepository dbSet = new SqlCounterpartyRepository();
                //dbSet.UpdateAsync(SelectedCounterparty.Counterparty);
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
            //SelectedCounterparty.Counterparty.Title = _oldCounterparty.Title;
            //SelectedCounterparty.Counterparty.Address = _oldCounterparty.Address;
            //SelectedCounterparty.Counterparty.ContactPerson = _oldCounterparty.ContactPerson;
            //SelectedCounterparty.Counterparty.ContactPhone = _oldCounterparty.ContactPhone;
            //SelectedCounterparty.Counterparty.Props = _oldCounterparty.Props;
            IsUpdate = false;
        }

        private void Update()
        {
            //_oldCounterparty = (Counterparty)SelectedCounterparty.Counterparty.Clone();
            IsUpdate = true;
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

using System;
using System.ComponentModel;
using System.Windows;
using ModelModul;
using ModelModul.Warranty;
using Prism.Commands;
using Prism.Regions;

namespace WarrantyModul.ViewModels
{
    class WarrantyInfoViewModel : ViewModelBase
    { 
        #region Properties

        private bool _isUpdate;

        public bool IsUpdate
        {
            get => _isUpdate;
            set => SetProperty(ref _isUpdate, value);
        }

        private readonly Warranties _oldWarranty = new Warranties();

        private WarrantyViewModel _selectedWarranty = new WarrantyViewModel();
        public WarrantyViewModel SelectedWarranty
        {
            get => _selectedWarranty;
            set
            {
                _selectedWarranty = value;
                IsUpdate = false;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }
        public DelegateCommand GoodsDepartureCommand { get; }
        public DelegateCommand GoodsIssueCommand { get; }

        #endregion

        public WarrantyInfoViewModel()
        {
            SelectedWarranty.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args) {RaisePropertyChanged(args.PropertyName); };
            GoodsDepartureCommand = new DelegateCommand(GoodsDeparture);
            GoodsIssueCommand = new DelegateCommand(GoodsIssue);
            UpdateCommand = new DelegateCommand(Update).ObservesCanExecute(() => SelectedWarranty.IsValidate);
            ResetCommand = new DelegateCommand(Reset);
            OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedWarranty.IsValidate);
        }
        
        private void Accept()
        {
            try
            {
                DbSetWarranties dbSet = new DbSetWarranties();
                dbSet.Update(SelectedWarranty.Warranty);
                MessageBox.Show("Информация о гарантии изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                IsUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset()
        {
            SelectedWarranty.Malfunction = _oldWarranty.Malfunction;
            SelectedWarranty.Info = _oldWarranty.Info;
            SelectedWarranty.DateIssue = _oldWarranty.DateIssue;
            SelectedWarranty.DateDeparture = _oldWarranty.DateDeparture;
            SelectedWarranty.DateReceipt = _oldWarranty.DateReceipt;
            IsUpdate = false;
        }

        private void Update()
        {
            _oldWarranty.Malfunction = SelectedWarranty.Malfunction;
            _oldWarranty.Info = SelectedWarranty.Info;
            _oldWarranty.DateIssue = SelectedWarranty.DateIssue;
            _oldWarranty.DateDeparture = SelectedWarranty.DateDeparture;
            _oldWarranty.DateReceipt = SelectedWarranty.DateReceipt;
            IsUpdate = true;
        }

        private void GoodsIssue()
        {
            try
            {
                DbSetWarranties dbSet = new DbSetWarranties();
                AutomationAccountingGoodsEntities db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString);
                db.GoodsIssued(SelectedWarranty.Warranty.Id);
                SelectedWarranty.Warranty = dbSet.Find(SelectedWarranty.Warranty.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoodsDeparture()
        {
            try
            {
                DbSetWarranties dbSet = new DbSetWarranties();
                AutomationAccountingGoodsEntities db = new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString);
                db.GoodsShipped(SelectedWarranty.Warranty.Id);
                SelectedWarranty.Warranty = dbSet.Find(SelectedWarranty.Warranty.Id);
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
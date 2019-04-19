using System;
using System.Collections.ObjectModel;
using System.Windows;
using EventAggregatorLibrary;
using ModelModul;
using ModelModul.Product;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace ProductModul.ViewModels
{
    class AddProductViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        readonly IEventAggregator _ea;

        private readonly DbSetWarrantyPeriodsModel _dbSetWarrantyPeriodsModel = new DbSetWarrantyPeriodsModel();

        public ObservableCollection<WarrantyPeriods> WarrantyPeriods => _dbSetWarrantyPeriodsModel.List;

        private readonly Products _product = new Products();

        public WarrantyPeriods WarrantyPeriod
        {
            get => _product.WarrantyPeriods;
            set
            {
                _product.WarrantyPeriods = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
            }
        }

        public string Title
        {
            get => _product.Title;
            set
            {
                _product.Title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
            }
        }

        public string Barcode
        {
            get => _product.Barcode;
            set
            {
                _product.Barcode = value;
                RaisePropertyChanged();
            }
        }

        public string VendorCode
        {
            get => _product.VendorCode;
            set
            {
                _product.VendorCode = value;
                RaisePropertyChanged();
            }
        }

        private bool IsEnabled => Title != "" && WarrantyPeriod != null;

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                Title = "";
                Barcode = "";
                VendorCode = "";
                WarrantyPeriod = null;
                LoadWarrantyPeriods();
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddProductCommand { get; }

        #endregion

        public AddProductViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<GroupsEventAggregator>().Subscribe(GetSelectedGroup);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabled);
        }

        private void GetSelectedGroup(Groups obj)
        {
            _product.Groups = obj;
        }

        private async void LoadWarrantyPeriods()
        {
            try
            {
                await _dbSetWarrantyPeriodsModel.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            RaisePropertyChanged("WarrantyPeriods");
        }


        public void AddProduct()
        {
            try
            {
                DbSetProductsModel dbSetProductsModel = new DbSetProductsModel();
                dbSetProductsModel.Add((Products)_product.Clone());
                MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
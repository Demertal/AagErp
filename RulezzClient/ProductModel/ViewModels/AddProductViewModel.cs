using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using ModelModul.Product;
using ModelModul.UnitStorage;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace ProductModul.ViewModels
{
    class AddProductViewModel : BindableBase, IInteractionRequestAware
    {
        #region GroupProperties

        private readonly DbSetGroups _groupModel = new DbSetGroups();

        public ObservableCollection<Groups> GroupsList => _groupModel.List;

        public DelegateCommand<Groups> SelectedGroupCommand { get; }

        #endregion

        #region ProductProperties

        private readonly DbSetWarrantyPeriods _warrantyPeriodsModel = new DbSetWarrantyPeriods();
        private readonly DbSetUnitStorages _unitStoragesModel = new DbSetUnitStorages();

        public ObservableCollection<WarrantyPeriods> WarrantyPeriods => _warrantyPeriodsModel.List;
        public ObservableCollection<UnitStorages> UnitStorages => _unitStoragesModel.List;

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

        public Groups SelectedGroup
        {
            get => _product.Groups;
            set
            {
                _product.Groups = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
            }
        }

        public UnitStorages UnitStorage
        {
            get => _product.UnitStorages;
            set
            {
                _product.UnitStorages = value;
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

        private bool IsEnabled => Title != "" && WarrantyPeriod != null && UnitStorage != null && SelectedGroup != null;

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
                UnitStorage = null;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddProductCommand { get; }

        #endregion

        public AddProductViewModel()
        {
            SelectedGroupCommand = new DelegateCommand<Groups>(SelectedGroupChange);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabled);
            Load();
        }

        #region GroupCommands

        private async void Load()
        {
            try
            {
                await _groupModel.Load();
                RaisePropertyChanged("GroupsList");
                await _warrantyPeriodsModel.Load();
                RaisePropertyChanged("WarrantyPeriods");
                await _unitStoragesModel.Load();
                RaisePropertyChanged("UnitStorages");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedGroupChange(Groups obj)
        {
            SelectedGroup = obj;
        }

        #endregion

        public void AddProduct()
        {
            try
            {
                DbSetProducts dbSetProducts = new DbSetProducts();
                dbSetProducts.Add((Products)_product.Clone());
                MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
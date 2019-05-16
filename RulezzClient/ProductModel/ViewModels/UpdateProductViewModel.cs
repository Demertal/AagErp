using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    class UpdateProductViewModel : BindableBase, IInteractionRequestAware
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

        private Products _product = new Products();

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
                _product = (Products)_notification.Content;
                Title = _product.Title;
                Barcode = _product.Barcode;
                VendorCode = _product.VendorCode;
                WarrantyPeriod = _product.WarrantyPeriods;
                UnitStorage = _product.UnitStorages;
                SelectedGroup = _product.Groups;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand UpdateProductCommand { get; }

        #endregion

        public UpdateProductViewModel()
        {
            LoadAsync();
            SelectedGroupCommand = new DelegateCommand<Groups>(SelectedGroupChange);
            UpdateProductCommand = new DelegateCommand(UpdateProduct).ObservesCanExecute(() => IsEnabled);
        }

        #region GroupCommands

        private async Task LoadAsync()
        {
            try
            {
                await _groupModel.LoadAsync();
                RaisePropertyChanged("GroupsList");
                await _warrantyPeriodsModel.LoadAsync();
                RaisePropertyChanged("WarrantyPeriods");
                await _unitStoragesModel.LoadAsync();
                RaisePropertyChanged("UnitStorages");
                RaisePropertyChanged("SelectedGroup");
                RaisePropertyChanged("WarrantyPeriod");
                RaisePropertyChanged("UnitStorage");
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

        public async void UpdateProduct()
        {
            try
            {
                DbSetProducts dbSetProducts = new DbSetProducts();
                await dbSetProducts.UpdateAsync((Products)_product.Clone());
                MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

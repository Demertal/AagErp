using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GenerationBarcodeLibrary;
using ModelModul;
using ModelModul.Product;
using ModelModul.UnitStorage;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class AddProductViewModel : ViewModelBase, IInteractionRequestAware
    {
        #region ProductProperties

        private ObservableCollection<WarrantyPeriods> _warrantyPeriods = new ObservableCollection<WarrantyPeriods>();
        public ObservableCollection<WarrantyPeriods> WarrantyPeriods
        {
            get => _warrantyPeriods;
            set => SetProperty(ref _warrantyPeriods, value);
        }

        private ObservableCollection<UnitStorages> _unitStorages = new ObservableCollection<UnitStorages>();
        public ObservableCollection<UnitStorages> UnitStorages
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
        }

        private ProductViewModel _product = new ProductViewModel();
        public ProductViewModel Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged();
            }
        }

        public string Group => Product?.Group == null ? "Группа:" : "Группа: " + Product.Group.Title;

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                Product = new ProductViewModel {Group = (Groups) _notification.Content};
                Product.IdGroup = Product.Group.Id;
                UnitStorages tempUnit = UnitStorages.FirstOrDefault(obj => obj.Title == "шт");
                Product.IdUnitStorage = tempUnit?.Id ?? 0;
                WarrantyPeriods tempPeriods = WarrantyPeriods.FirstOrDefault(obj => obj.Period == "Нет");
                Product.IdWarrantyPeriod = tempPeriods?.Id ?? 0;
                RaisePropertyChanged("Group");
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }

        #endregion

        public AddProductViewModel()
        {
            Load();
            Product.PropertyChanged += (o, e) => RaisePropertyChanged(e.PropertyName);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => Product.IsValidate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
        }

        private void Load()
        {
            try
            {
                DbSetUnitStorages dbSetUnitStorages = new DbSetUnitStorages();
                UnitStorages = dbSetUnitStorages.Load();
                DbSetWarrantyPeriods dbSetWarrantyPeriods = new DbSetWarrantyPeriods();
                WarrantyPeriods = dbSetWarrantyPeriods.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddProduct()
        {
            Groups temp = _product.Group;
            try
            {
                _product.Group = null;
                _product.CountProducts = null;
                _product.PropertyProducts = null;
                _product.UnitStorage = null;
                _product.WarrantyPeriod = null;
                DbSetProducts dbSetProducts = new DbSetProducts();
                dbSetProducts.Add((Products)_product.Product.Clone());
                MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                _product.Group = temp;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateBarcode()
        {
            try
            {
                DbSetProducts dbSetProducts = new DbSetProducts();
                string temp;
                do
                {
                    temp = GenerationBarcode.GenerateBarcode();
                } while (dbSetProducts.CheckBarcode(temp));

                Product.Barcode = temp;
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
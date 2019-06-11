using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GenerationBarcodeLibrary;
using ModelModul;
using ModelModul.Product;
using ModelModul.PropertyProduct;
using ModelModul.UnitStorage;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class ProductInfoViewModel : ViewModelBase
    {
        #region ProductProperties

        private bool _isUpdate;

        public bool IsUpdate
        {
            get => _isUpdate;
            set => SetProperty(ref _isUpdate, value);
        }

        private Products _oldProduct = new Products();

        private ObservableCollection<WarrantyPeriods> _warrantyPeriods = new ObservableCollection<WarrantyPeriods>();
        public ObservableCollection<WarrantyPeriods> WarrantyPeriods
        {
            get => _warrantyPeriods;
            set
            {
                SetProperty(ref _warrantyPeriods, value);
                RaisePropertyChanged("IdWarrantyPeriod");
            }
        }

        private ObservableCollection<UnitStorages> _unitStorages = new ObservableCollection<UnitStorages>();
        public ObservableCollection<UnitStorages> UnitStorages
        {
            get => _unitStorages;
            set
            {
                SetProperty(ref _unitStorages, value);
                RaisePropertyChanged("IdUnitStorage");
            }
        }

        private ObservableCollection<string> _countProducts = new ObservableCollection<string>();
        public ObservableCollection<string> CountProducts
        {
            get => _countProducts;
            set => SetProperty(ref _countProducts, value);
        }

        private ObservableCollection<PropertyProducts> _propertyProductsList = new ObservableCollection<PropertyProducts>();
        public ObservableCollection<PropertyProducts> PropertyProductsList
        {
            get => _propertyProductsList;
            set => SetProperty(ref _propertyProductsList, value);
        }

        private ProductViewModel _selectedProduct = new ProductViewModel();
        public ProductViewModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                IsUpdate = false;
                Load();
                _selectedProduct.Product = _selectedProduct.Product;
                GetCountProduct();
                RaisePropertyChanged();
                RaisePropertyChanged("Group");
            }
        }

        public string Group => SelectedProduct?.Group == null ? "Группа:" : "Группа: " + SelectedProduct.Group.Title;

        public int IdWarrantyPeriod
        {
            get => SelectedProduct.IdWarrantyPeriod;
            set
            {
                SelectedProduct.IdWarrantyPeriod = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public int IdUnitStorage
        {
            get => SelectedProduct.IdUnitStorage;
            set
            {
                SelectedProduct.IdUnitStorage = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public DelegateCommand UpdateProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public ProductInfoViewModel()
        {
            SelectedProduct.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                RaisePropertyChanged(args.PropertyName);
            };
            UpdateProductCommand = new DelegateCommand(UpdateProduct).ObservesCanExecute(() => SelectedProduct.IsValidate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ResetCommand = new DelegateCommand(Reset);
            OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedProduct.IsValidate); 
        }

        private void Load()
        {
            try
            {
                DbSetUnitStorages dbSetUnitStorages = new DbSetUnitStorages();
                UnitStorages = dbSetUnitStorages.Load();
                DbSetWarrantyPeriods dbSetWarrantyPeriods = new DbSetWarrantyPeriods();
                WarrantyPeriods = dbSetWarrantyPeriods.Load();
                DbSetPropertyProducts dbSetPropertyProducts = new DbSetPropertyProducts();
                PropertyProductsList = dbSetPropertyProducts.Load(SelectedProduct.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetCountProduct()
        {
            try
            {
                DbSetProducts dbSet = new DbSetProducts();
                CountProducts = dbSet.GetCountProduct(SelectedProduct.Product.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateProduct()
        {
            _oldProduct = (Products)SelectedProduct.Product.Clone();
            IsUpdate = true;
        }

        private void Accept()
        {
            try
            {
                Products pr = (Products)SelectedProduct.Product.Clone();
                pr.Groups = null;
                pr.CountProducts = null;
                pr.PropertyProducts = null;
                pr.PurchaseInfos = null;
                pr.RevaluationProductsInfos = null;
                pr.SalesInfos = null;
                pr.SerialNumbers = null;
                pr.UnitStorages = null;
                pr.WarrantyPeriods = null;
                DbSetProducts dbSet = new DbSetProducts();
                dbSet.Update(pr);
                DbSetPropertyProducts dbSetProperty = new DbSetPropertyProducts();
                dbSetProperty.Update(PropertyProductsList.ToList());
                MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                IsUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset()
        {
            SelectedProduct.Product = _oldProduct;
            IsUpdate = false;
            Load();
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

                SelectedProduct.Barcode = temp;
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

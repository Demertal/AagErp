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
using Prism.Mvvm;

namespace ProductModul.ViewModels
{
    class ProductInfoViewModel : BindableBase
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
                LoadAsync();
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

        private async void LoadAsync()
        {
            try
            {
                DbSetUnitStorages dbSetUnitStorages = new DbSetUnitStorages();
                UnitStorages = await dbSetUnitStorages.LoadAsync();
                DbSetWarrantyPeriods dbSetWarrantyPeriods = new DbSetWarrantyPeriods();
                WarrantyPeriods = await dbSetWarrantyPeriods.LoadAsync();
                DbSetPropertyProducts dbSetPropertyProducts = new DbSetPropertyProducts();
                PropertyProductsList = await dbSetPropertyProducts.LoadAsync(SelectedProduct.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GetCountProduct()
        {
            try
            {
                DbSetProducts dbSet = new DbSetProducts();
                CountProducts = await dbSet.GetCountProduct(SelectedProduct.Product.Id);
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

        private async void Accept()
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
                await dbSet.UpdateAsync(pr);
                DbSetPropertyProducts dbSetProperty = new DbSetPropertyProducts();
                await dbSetProperty.UpdateAsync(PropertyProductsList.ToList());
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
            LoadAsync();
        }

        private async void GenerateBarcode()
        {
            try
            {
                DbSetProducts dbSetProducts = new DbSetProducts();
                string temp;
                do
                {
                    temp = GenerationBarcode.GenerateBarcode();
                } while (await dbSetProducts.CheckBarcodeAsync(temp) != 0);

                SelectedProduct.Barcode = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

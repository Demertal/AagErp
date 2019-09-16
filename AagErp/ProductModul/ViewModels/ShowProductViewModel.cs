using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using GenerationBarcodeLibrary;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : DialogViewModelBase, IEditableObject
    {
        #region ProductProperties

        private ObservableCollection<WarrantyPeriod> _warrantyPeriodsList = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriodsList;
            set => SetProperty(ref _warrantyPeriodsList, value);
        }

        private ObservableCollection<UnitStorage> _unitStoragesList = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStoragesList;
            set => SetProperty(ref _unitStoragesList, value);
        }

        private ObservableCollection<PriceGroup> _priceGroupsListList = new ObservableCollection<PriceGroup>();
        public ObservableCollection<PriceGroup> PriceGroupsList
        {
            get => _priceGroupsListList;
            set => SetProperty(ref _priceGroupsListList, value);
        }

        private ObservableCollection<PropertyProduct> _propertyProductsList = new ObservableCollection<PropertyProduct>();
        public ObservableCollection<PropertyProduct> PropertyProductsList
        {
            get => _propertyProductsList;
            set => SetProperty(ref _propertyProductsList, value);
        }

        private Product _product = new Product();
        public Product Product
        {
            get => _product;
            set
            {
                SetProperty(ref _product, value);
                _product.PropertyChanged += (o, e) => { RaisePropertyChanged(e.PropertyName); };
                RaisePropertyChanged("CanEdit");
            }
        }

        private bool _isAdd;
        public bool IsAdd
        {
            get => _isAdd;
            set => SetProperty(ref _isAdd, value);
        }

        public DelegateCommand UpdateProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public ShowProductViewModel()
        {
            UpdateProductCommand = new DelegateCommand(BeginEdit);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ResetCommand = new DelegateCommand(CancelEdit);
            OkCommand = new DelegateCommand(EndEdit).ObservesCanExecute(() => Product.IsValidate);
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                SqlProductRepository productForCountRepository = new SqlProductRepository();
                SqlProductRepository productForPropertyRepository = new SqlProductRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => warrantyPeriodRepository.GetListAsync());
                var priceGroupsLoad = Task.Run(() => priceGroupRepository.GetListAsync());
                var loadCount = Task.Run(() => productForCountRepository.GetCountsProduct(Product.Id));
                var loadProperty = Task.Run(() => productForPropertyRepository.GetPropertyForProduct(Product));

                Task.WaitAll(loadUnitStorage, loadWarrantyPeriod, priceGroupsLoad, loadCount, loadProperty);

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                PriceGroupsList = new ObservableCollection<PriceGroup>(priceGroupsLoad.Result);
                Product.CountsProductCollection = new ObservableCollection<CountsProduct>(loadCount.Result);
                Product.PropertyProductsCollection = new ObservableCollection<PropertyProduct>(loadProperty.Result);
                foreach (var propertyProduct in Product.PropertyProductsCollection)
                {
                    propertyProduct.IdProduct = Product.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GenerateBarcode()
        {
            try
            {
                IRepository<Product> sqlProductRepository = new SqlProductRepository();
                string temp;
                do
                {
                    temp = GenerationBarcode.GenerateBarcode();
                } while (await sqlProductRepository.AnyAsync(ProductSpecification.GetProductsByBarcode(temp)));

                Product.Barcode = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                _backup = parameters.GetValue<Product>("product");
                IsAdd = IsEdit = false;
                if (_backup != null)
                {
                    Product = (Product) _backup.Clone();
                    Title = Product.Title;
                }
                else
                {
                    Title = "Добавить товар";
                    Product = new Product { Category = parameters.GetValue<Category>("category") };
                    Product.IdCategory = Product.Category.Id;
                    IsAdd = IsEdit = true;
                }
                LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region EditableObject

        private Product _backup = new Product();

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public void BeginEdit()
        {
            IsEdit = true;
            RaisePropertyChanged("Product");
        }

        public async void EndEdit()
        {
            try
            {
                string message = "Товар изменен";
                Product temp = (Product) Product.Clone();
                temp.Category = null;
                IRepository<Product> sqlProductRepository = new SqlProductRepository();
                if (Product.Id == 0)
                {
                    await sqlProductRepository.CreateAsync(temp);
                    Product.Id = temp.Id;
                    message = "Товар добавлен";
                }
                else
                {
                    await sqlProductRepository.UpdateAsync(Product);
                    _backup.Barcode = Product.Barcode;
                    _backup.IdCategory = Product.IdCategory;
                    _backup.IdPriceGroup = Product.IdPriceGroup;
                    _backup.IdUnitStorage = Product.IdUnitStorage;
                    _backup.IdWarrantyPeriod = Product.IdWarrantyPeriod;
                    _backup.KeepTrackSerialNumbers = Product.KeepTrackSerialNumbers;
                    _backup.Title = Product.Title;
                    _backup.VendorCode = Product.VendorCode;
                    _backup.Description = Product.Description;
                    IsEdit = false;
                }
                //SqlPropertyProductRepository sqlProperty = new SqlPropertyProductRepository();
                //sqlProperty.UpdateAsync(PropertyProductsList.ToList());
                MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if(IsAdd)
                    RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "product", Product } }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CancelEdit()
        {
            Product.Barcode = _backup.Barcode;
            Product.IdCategory = _backup.IdCategory;
            Product.IdPriceGroup = _backup.IdPriceGroup;
            Product.IdUnitStorage = _backup.IdUnitStorage;
            Product.IdWarrantyPeriod = _backup.IdWarrantyPeriod;
            Product.KeepTrackSerialNumbers = _backup.KeepTrackSerialNumbers;
            Product.Title = _backup.Title;
            Product.VendorCode = _backup.VendorCode;
            Product.Description = _backup.Description;
            IsEdit = false;
        }

        #endregion
    }
}

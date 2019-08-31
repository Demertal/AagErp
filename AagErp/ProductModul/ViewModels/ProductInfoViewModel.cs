using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CustomControlLibrary.MVVM;
using GenerationBarcodeLibrary;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    class ProductInfoViewModel : DialogViewModelBase, IEditableObject
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

        private Product _selectedProduct = new Product();
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                _selectedProduct.PropertyChanged += (o, e) => { RaisePropertyChanged(e.PropertyName); };
                RaisePropertyChanged("CanEdit");
            }
        }

        public DelegateCommand UpdateProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }

        #endregion

        public ProductInfoViewModel()
        {
            UpdateProductCommand = new DelegateCommand(BeginEdit).ObservesCanExecute(() => CanEdit);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ResetCommand = new DelegateCommand(CancelEdit);
            OkCommand = new DelegateCommand(EndEdit).ObservesCanExecute(() => SelectedProduct.IsValidate);
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<UnitStorage> sqlUnitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                IRepository<PriceGroup> sqlPriceGroupRepository = new SqlPriceGroupRepository();
                IRepository<PropertyProduct> sqlPropertyProductRepository = new SqlPropertyProductRepository();
                SqlProductRepository sqlProductRepository = new SqlProductRepository();

                var loadUnitStorage = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());
                var priceGroupsLoad = Task.Run(() => sqlPriceGroupRepository.GetListAsync());
                var loadCount = Task.Run(() => sqlProductRepository.GetCountsProduct(SelectedProduct.Id));

                Task.WaitAll(loadUnitStorage, loadWarrantyPeriod, priceGroupsLoad, loadCount);

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                PriceGroupsList = new ObservableCollection<PriceGroup>(priceGroupsLoad.Result);
                SelectedProduct.CountsProductCollection = new ObservableCollection<CountsProduct>(loadCount.Result);
                //PropertyProductsList = new ObservableCollection<PropertyProduct>(await sqlPropertyProductRepository.GetListAsync(PropertyProductSpecification.GetPropertyProductsByIdProduct(SelectedProduct.Id)));
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

                SelectedProduct.Barcode = temp;
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
                SelectedProduct = parameters.GetValue<Product>("product");
                Title = SelectedProduct.Title;
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

        public bool CanEdit => SelectedProduct != null && SelectedProduct.Id != 0;

        public void BeginEdit()
        {
            _backup = (Product)SelectedProduct?.Clone();
            IsEdit = true;
            RaisePropertyChanged("SelectedProduct");
        }

        public async void EndEdit()
        {
            try
            {
                Product pr = (Product)SelectedProduct.Clone();
                IRepository<Product> sqlProductRepository = new SqlProductRepository();
                await sqlProductRepository.UpdateAsync(pr);
                //SqlPropertyProductRepository sqlProperty = new SqlPropertyProductRepository();
                //sqlProperty.UpdateAsync(PropertyProductsList.ToList());
                MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                IsEdit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CancelEdit()
        {
            SelectedProduct.Barcode = _backup.Barcode;
            SelectedProduct.IdCategory = _backup.IdCategory;
            SelectedProduct.IdPriceGroup = _backup.IdPriceGroup;
            SelectedProduct.IdUnitStorage = _backup.IdUnitStorage;
            SelectedProduct.IdWarrantyPeriod = _backup.IdWarrantyPeriod;
            SelectedProduct.KeepTrackSerialNumbers = _backup.KeepTrackSerialNumbers;
            SelectedProduct.Title = _backup.Title;
            SelectedProduct.VendorCode = _backup.VendorCode;
            SelectedProduct.Description = _backup.Description;
            IsEdit = false;
        }

        #endregion
    }
}

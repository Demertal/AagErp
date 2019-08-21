using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CustomControlLibrary.MVVM;
using GenerationBarcodeLibrary;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Events;
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

        private bool _isAddPurchase;
        public bool IsAddPurchase
        {
            get => _isAddPurchase;
            set => SetProperty(ref _isAddPurchase, value);
        }

        IEventAggregator _ea;

        private Product _oldProduct = new Product();

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
                LoadAsync();
                IsUpdate = false;
                _selectedProduct = value;
                _selectedProduct.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
                {
                    RaisePropertyChanged(args.PropertyName);
                };
                GetCountProduct();
                RaisePropertyChanged();
                RaisePropertyChanged("CanUpdate");
            }
        }

        public string Category => SelectedProduct?.Category == null ? "Категория:" : "Категория: " + SelectedProduct.Category.Title;

        public bool CanUpdate => SelectedProduct != null && SelectedProduct.Id != 0;

        public DelegateCommand UpdateProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand OkCommand { get; }
        public DelegateCommand LoadedCommand { get; }

        #endregion

        public ProductInfoViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<BoolSentEvent>().Subscribe(MessageReceived);
            UpdateProductCommand = new DelegateCommand(UpdateProduct).ObservesCanExecute(() => CanUpdate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ResetCommand = new DelegateCommand(Reset);
            OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedProduct.IsValidate);
            LoadedCommand = new DelegateCommand(Loaded);
        }

        private void Loaded()
        {
            _ea.GetEvent<IsReadySentEvent>().Publish(true);
        }

        private void MessageReceived(bool obj)
        {
            IsAddPurchase = obj;
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<UnitStorage> sqlUnitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                IRepository<PropertyProduct> sqlPropertyProductRepository = new SqlPropertyProductRepository();

                var loadUnitStorage = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());

                Task.WaitAll(loadUnitStorage, loadWarrantyPeriod);

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
               //PropertyProductsList = new ObservableCollection<PropertyProduct>(await sqlPropertyProductRepository.GetListAsync(PropertyProductSpecification.GetPropertyProductsByIdProduct(SelectedProduct.Id)));
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
                if (SelectedProduct == null) return;
                SqlProductRepository sqlProductRepository = new SqlProductRepository();
                SelectedProduct.CountsProduct =
                    new ObservableCollection<CountsProduct>(
                        await sqlProductRepository.GetCountsProduct(SelectedProduct.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateProduct()
        {
            _oldProduct = (Product)SelectedProduct?.Clone();
            IsUpdate = true;
        }

        private async void Accept()
        {
            try
            {
                Product pr = (Product)SelectedProduct.Clone();
                IRepository<Product> sqlProductRepository = new SqlProductRepository();
                await sqlProductRepository.UpdateAsync(pr);
                //SqlPropertyProductRepository sqlProperty = new SqlPropertyProductRepository();
                //sqlProperty.UpdateAsync(PropertyProductsList.ToList());
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
            SelectedProduct.Barcode = _oldProduct.Barcode;
            SelectedProduct.IdCategory = _oldProduct.IdCategory;
            SelectedProduct.IdPriceGroup = _oldProduct.IdPriceGroup;
            SelectedProduct.IdUnitStorage = _oldProduct.IdUnitStorage;
            SelectedProduct.IdWarrantyPeriod = _oldProduct.IdWarrantyPeriod;
            SelectedProduct.KeepTrackSerialNumbers = _oldProduct.KeepTrackSerialNumbers;
            SelectedProduct.Title = _oldProduct.Title;
            SelectedProduct.VendorCode = _oldProduct.VendorCode;
            IsUpdate = false;
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

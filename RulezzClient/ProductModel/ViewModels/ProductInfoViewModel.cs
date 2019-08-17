using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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

        private ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriods
        {
            get => _warrantyPeriods;
            set
            {
                SetProperty(ref _warrantyPeriods, value);
                RaisePropertyChanged("IdWarrantyPeriod");
            }
        }

        private ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStorages
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

        private ObservableCollection<PropertyProduct> _propertyProductsList = new ObservableCollection<PropertyProduct>();
        public ObservableCollection<PropertyProduct> PropertyProductsList
        {
            get => _propertyProductsList;
            set => SetProperty(ref _propertyProductsList, value);
        }

        //private ProductViewModel _selectedProduct = new ProductViewModel();
        //public ProductViewModel SelectedProduct
        //{
        //    get => _selectedProduct;
        //    set
        //    {
        //        _selectedProduct = new ProductViewModel{Id = value .Id};
        //        LoadAsync();
        //        IsUpdate = false;
        //        _selectedProduct = value;
        //        _selectedProduct.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
        //        {
        //            RaisePropertyChanged(args.PropertyName);
        //        };
        //        _selectedProduct.Product = _selectedProduct.Product;
        //        GetCountProduct();
        //        RaisePropertyChanged();
        //        RaisePropertyChanged("Parent");
        //    }
        //}

        //public string Group => SelectedProduct?.Category == null ? "Группа:" : "Группа: " + SelectedProduct.Category.Title;

        //public int IdWarrantyPeriod
        //{
        //    get => SelectedProduct.IdWarrantyPeriod;
        //    set
        //    {
        //        SelectedProduct.IdWarrantyPeriod = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged("IsValidate");
        //    }
        //}

        //public int IdUnitStorage
        //{
        //    get => SelectedProduct.IdUnitStorage;
        //    set
        //    {
        //        SelectedProduct.IdUnitStorage = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged("IsValidate");
        //    }
        //}

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
            //UpdateProductCommand = new DelegateCommand(UpdateProduct).ObservesCanExecute(() => SelectedProduct.IsValidate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ResetCommand = new DelegateCommand(Reset);
            //OkCommand = new DelegateCommand(Accept).ObservesCanExecute(() => SelectedProduct.IsValidate);
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

        private async void LoadAsync()
        {
            try
            {
                SqlUnitStorageRepository sqlUnitStorageRepository = new SqlUnitStorageRepository();
                //UnitStorages = new ObservableCollection<UnitStorage>(await sqlUnitStorageRepository.GetListAsync());
                SqlWarrantyPeriodRepository sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                //WarrantyPeriods = new ObservableCollection<WarrantyPeriod>(await sqlWarrantyPeriodRepository.GetListAsync());
                SqlPropertyProductRepository sqlPropertyProductRepository = new SqlPropertyProductRepository();
                //PropertyProductsList = new ObservableCollection<PropertyProduct>(await sqlPropertyProductRepository.GetListAsync(PropertyProductSpecification.GetPropertyProductsByIdProduct(SelectedProduct.Id)));
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
                SqlProductRepository dbSet = new SqlProductRepository();
                //CountProducts = dbSet.GetCountAsync(SelectedProduct.Product.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateProduct()
        {
            //_oldProduct = (Product)SelectedProduct.Product.Clone();
            IsUpdate = true;
        }

        private async void Accept()
        {
            try
            {
                //Product pr = (Product)SelectedProduct.Product.Clone();
                //pr.Category = null;
                //pr.CountProducts = null;
                //pr.PropertyProducts = null;
                //pr.MovementGoodsInfos = null;
                //pr.PriceProducts = null;
                //pr.PriceGroup = null;
                //pr.SerialNumbers = null;
                //pr.UnitStorage = null;
                //pr.WarrantyPeriod = null;
                //SqlProductRepository dbSet = new SqlProductRepository();
                //await dbSet.UpdateAsync(pr);
                //SqlPropertyProductRepository sqlProperty = new SqlPropertyProductRepository();
                ////sqlProperty.UpdateAsync(PropertyProductsList.ToList());
                //MessageBox.Show("Товар изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                //IsUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset()
        {
            //SelectedProduct.Product = _oldProduct;
            IsUpdate = false;
            LoadAsync();
        }

        private void GenerateBarcode()
        {
            try
            {
                SqlProductRepository sqlProductRepository = new SqlProductRepository();
                string temp;
                //do
                //{
                //    temp = GenerationBarcode.GenerateBarcode();
                //} while (sqlProductRepository.CheckBarcode(temp));

                //SelectedProduct.Barcode = temp;
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

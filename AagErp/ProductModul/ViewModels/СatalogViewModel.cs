using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    public class СatalogViewModel : ViewModelBase, IDialogAware
    {
        private CancellationTokenSource _cancelTokenProduct;
        private CancellationTokenSource _cancelTokenProperty;

        #region Purchase

        private bool _isAddPurchase;
        public bool IsAddPurchase
        {
            get => _isAddPurchase;
            set => SetProperty(ref _isAddPurchase, value);
        }

        #endregion

        #region DirectoryProperties

        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();

        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        private ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriods;
            set => SetProperty(ref _warrantyPeriods, value);
        }

        private ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
        }

        private ObservableCollection<PropertyProduct> _propertyProductsList;
        public ObservableCollection<PropertyProduct> PropertyProductsList
        {
            get => _propertyProductsList;
            set => SetProperty(ref _propertyProductsList, value);
        }

        #endregion

        #region FilterProperties

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                LoadPropertyAsync();
                LoadAsync();
                RaisePropertyChanged("IsEnabledAddProduct");
            }
        }

        private string _findString;
        public string FindString
        {
            get => _findString;
            set
            {
                SetProperty(ref _findString, value);
                LoadAsync();
            }
        }

        #endregion

        #region ProductProperties
        
        private ObservableCollection<Product> _productsList = new ObservableCollection<Product>();
        public ObservableCollection<Product> ProductsList
        {
            get => _productsList;
            set => SetProperty(ref _productsList, value);
        }

        #endregion

        #region Delegate

        public DelegateCommand<Product> SelectedProductCommand { get; }

        public DelegateCommand ResetCommand { get; }

        #endregion

        public СatalogViewModel(IDialogService dialogService) : base(dialogService)
        {
            IsAddPurchase = false;
            SelectedProductCommand = new DelegateCommand<Product>(SelectedProduct);
            ResetCommand = new DelegateCommand(Reset);
        }

        #region CategoryCommands

        private async void PreLoadAsync()
        {
            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => warrantyPeriodRepository.GetListAsync());
                var loadCategory = Task.Run(() => categoryRepository.GetListAsync(include: (c => c.ChildCategoriesCollection, null)));

                await Task.WhenAll(loadUnitStorage, loadWarrantyPeriod, loadCategory);
                CategoriesList = new ObservableCollection<Category>((await loadCategory).Where(CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy().Compile()));
                UnitStoragesList = new ObservableCollection<UnitStorage>(await loadUnitStorage);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(await loadWarrantyPeriod);
                LoadPropertyAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ProductCommands

        private async void LoadPropertyAsync()
        {
            _cancelTokenProperty?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenProperty = newCts;

            try
            {
                SqlProductRepository productRepository = new SqlProductRepository();
                PropertyProductsList = new ObservableCollection<PropertyProduct>(
                    await productRepository.GetPropertyForProduct(SelectedCategory?.Id, cts: _cancelTokenProperty.Token));

                foreach (var propertyProduct in PropertyProductsList)
                {
                    propertyProduct.PropertyChanged += (o, e) => { LoadAsync(); };
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (_cancelTokenProperty == newCts)
                _cancelTokenProperty = null;
        }

        private async void LoadAsync()
        {
            _cancelTokenProduct?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenProduct = newCts;

            try
            {
                SqlProductRepository sqlProductRepository = new SqlProductRepository();
                
                ProductsList = new ObservableCollection<Product>(
                    await sqlProductRepository.GetProductsWithCountAndPrice(_cancelTokenProduct.Token,
                        ProductSpecification.GetProductsByIdGroupOrFindStringOrProperty(SelectedCategory?.Id,
                            FindString, PropertyProductsList)));
            }
            catch (OperationCanceledException) {}
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (_cancelTokenProduct == newCts)
                _cancelTokenProduct = null;
        }

        private void SelectedProduct(Product obj)
        {
            if (IsAddPurchase)
                RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "product", obj } }));
            else
                DialogService.Show("ShowProduct", new DialogParameters { { "entity", obj }, { "isCatalog", true } }, null);
        }

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            PreLoadAsync();
            LoadAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        private void Reset()
        {
            _findString = null;
            _propertyProductsList = null;
            SelectedCategory = null;
            RaisePropertyChanged("FindString");
            RaisePropertyChanged("PropertyProductsList");
        }

        #region IDialogAware

        private string _iconSource;
        public string IconSource
        {
            get => _iconSource;
            set => SetProperty(ref _iconSource, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public event Action<IDialogResult> RequestClose;

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Title = "Выборать товар";
            IsAddPurchase = true;
            PreLoadAsync();
            LoadAsync();
        }

        #endregion
    }
}

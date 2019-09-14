using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    public class СatalogViewModel : ViewModelBase, IDialogAware
    {
        private readonly IDialogService _dialogService;

        #region Purchase

        private bool _isAddPurchase;
        public bool IsAddPurchase
        {
            get => _isAddPurchase;
            set => SetProperty(ref _isAddPurchase, value);
        }

        #endregion

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

        #region ProductProperties

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
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

        private ObservableCollection<Product> _productsList = new ObservableCollection<Product>();
        public ObservableCollection<Product> ProductsList
        {
            get => _productsList;
            set => SetProperty(ref _productsList, value);
        }
        
        public DelegateCommand<Product> SelectedProductCommand { get; }

        public bool IsEnabledAddProduct => SelectedCategory != null;
        #endregion

        public СatalogViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            IsAddPurchase = false;
            SelectedProductCommand = new DelegateCommand<Product>(SelectedProduct);
        }

        #region CategoryCommands

        private void PreLoadAsync()
        {
            try
            {
                IRepository<Category> saCategoryRepository = new SqlCategoryRepository();
                IRepository<UnitStorage> sqlUnitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();

                var loadUnitStorage = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());
                var loadCategory = Task.Run(() => saCategoryRepository.GetListAsync(include: c => c.ChildCategoriesCollection));

                Task.WaitAll(loadUnitStorage, loadWarrantyPeriod, loadCategory);

                CategoriesList = new ObservableCollection<Category>(loadCategory.Result.Where(CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy().Compile()));
                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ProductCommands

        private async void LoadAsync()
        {
            try
            {
                SqlProductRepository sqlProductRepository = new SqlProductRepository();

                ProductsList = new ObservableCollection<Product>(
                    await sqlProductRepository.GetProductsWithCountAndPrice(
                        ProductWithCountAndPriceSpecification.GetProductsByIdGroupOrFindString(SelectedCategory?.Id,
                            FindString), null, 0, -1, p => p.UnitStorage, p => p.Category));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedProduct(Product obj)
        {
            if (IsAddPurchase)
                RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "product", obj } }));
            else
                _dialogService.Show("ShowProduct", new DialogParameters { { "product", obj } }, null);
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

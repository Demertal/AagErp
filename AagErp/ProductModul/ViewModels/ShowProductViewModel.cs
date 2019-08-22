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
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    public class ShowProductViewModel : ViewModelBase, IDialogAware
    {
        private readonly IRegionManager _regionManager;

        private IEventAggregator _ea;

        private readonly IDialogService _dialogService;

        #region Purchase

        private bool _isAddPurchase;
        public bool IsAddPurchase
        {
            get => _isAddPurchase;
            set => SetProperty(ref _isAddPurchase, value);
        }

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                IsAddPurchase = true;
                FindString = "";
                SelectedCategory = null;
                ProductsList = new ObservableCollection<Product>();
                LoadCategoryAsync();
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand<Product> AddCommandPurchaseGoods { get; }

        #endregion

        #region CategoryProperties

        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();

        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        public DelegateCommand<Category> SelectedCategoryCommand { get; }

        public DelegateCommand<Category> DeleteCategoryCommand { get; }
        public DelegateCommand<Category> AddCategoryCommand { get; }
        public DelegateCommand<Category> RenameCategoryCommand { get; }
        public DelegateCommand<Category> ShowPropertiesCommand { get; }

        #endregion

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
                LoadProductAsync();
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
                LoadProductAsync();
            }
        }

        private ObservableCollection<Product> _productsList = new ObservableCollection<Product>();
        public ObservableCollection<Product> ProductsList
        {
            get => _productsList;
            set => SetProperty(ref _productsList, value);
        }

        public DelegateCommand<Product> DeleteProductCommand { get; }
        public DelegateCommand AddProductCommand { get; }

        public bool IsEnabledAddProduct => SelectedCategory != null;
        #endregion

        public ShowProductViewModel()
        {

        }

        public ShowProductViewModel(IRegionManager regionManager, IEventAggregator ea, IDialogService dialogService)
        {
            _ea = ea;
            _regionManager = regionManager;
            _dialogService = dialogService;
            IsAddPurchase = false;
            SelectedCategoryCommand = new DelegateCommand<Category>(SelectedGroupChange);
            AddCategoryCommand = new DelegateCommand<Category>(AddCategory);
            RenameCategoryCommand = new DelegateCommand<Category>(RenameCategory);
            DeleteCategoryCommand = new DelegateCommand<Category>(DeleteCategoryAsync);
            ShowPropertiesCommand = new DelegateCommand<Category>(ShowProperties);
            _ea.GetEvent<IsReadySentEvent>().Subscribe(obj => _ea.GetEvent<BoolSentEvent>().Publish(IsAddPurchase));

            DeleteProductCommand = new DelegateCommand<Product>(DeleteProductAsync);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabledAddProduct);

            AddCommandPurchaseGoods = new DelegateCommand<Product>(AddPurchaseGoods);
        }

        #region CategoryCommands

        private void LoadCategoryAsync()
        {
            try
            {
                SqlRepository<Category> saCategoryRepository = new SqlCategoryRepository();
                SqlRepository<UnitStorage> sqlUnitStorageRepository = new SqlUnitStorageRepository();
                SqlRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();

                var loadUnitStorage = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());
                var loadCategory = Task.Run(() => saCategoryRepository.GetListAsync(include: c => c.ChildCategories));

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

        private void SelectedGroupChange(Category obj)
        {
            SelectedCategory = obj;
        }

        private void AddCategory(Category category)
        {
            _dialogService.ShowDialog("AddCategory",
                new DialogParameters { { "id", category?.Id } },
                Callback);
        }

        private void Callback(IDialogResult dialogResult)
        {
            LoadCategoryAsync();
            LoadProductAsync();
        }

        private void RenameCategory(Category category)
        {
            _dialogService.ShowDialog("RenameCategory", new DialogParameters { { "category", category } }, Callback);
        }

        private void ShowProperties(Category category)
        {
            //ShowPropertiesPopupRequest.Raise(new Notification { Title = "Свойства", Content = category });
        }

        private async void DeleteCategoryAsync(Category category)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите удалить категорию? При удалении категории будут также удалены все дочерние категории, товар в них и свойства!", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                    MessageBoxResult.Yes) return;
                SqlRepository<Category> sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.DeleteAsync(category);
                MessageBox.Show("Категория удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategoryAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ProductCommands

        private async void LoadProductAsync()
        {
            try
            {
               SqlProductRepository sqlProductRepository = new SqlProductRepository();

                ProductsList = new ObservableCollection<Product>( await sqlProductRepository.GetListAsync(SelectedCategory == null
                    ? ProductSpecification.GetProductsByFindString(FindString)
                    : ProductSpecification.GetProductsByIdGroup(SelectedCategory.Id)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            _dialogService.ShowDialog("AddProduct", new DialogParameters { { "category", SelectedCategory } }, Callback);
        }

        private async void DeleteProductAsync(Product obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить товар?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlProductRepository dbSet = new SqlProductRepository();
                await dbSet.DeleteAsync(obj);
                MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProductAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadCategoryAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("ProductInfo");
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

        }

        #endregion

        private void AddPurchaseGoods(Product obj)
        {
            if (_notification != null)
            {
                _notification.Confirmed = true;
                _notification.Content = obj;
            }
            FinishInteraction?.Invoke();
        }
    }
}

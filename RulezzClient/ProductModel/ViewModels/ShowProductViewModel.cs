using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly IRegionManager _regionManager;

        private IEventAggregator _ea;

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
                Category = null;
                ProductsList = new ObservableCollection<Product>();
                LoadGroupAsync();
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand<Product> AddCommandPurchaseGoods { get; }

        #endregion

        #region GroupProperties

        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();

        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        public DelegateCommand<Category> SelectedGroupCommand { get; }

        public InteractionRequest<INotification> AddStorePopupRequest { get; set; }
        public InteractionRequest<INotification> AddGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> RenameGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> ShowPropertiesPopupRequest { get; set; }
        public DelegateCommand<Category> DeleteGroupCommand { get; }
        public DelegateCommand AddStoreCommand { get; }
        public DelegateCommand<Category> AddGroupCommand { get; }
        public DelegateCommand<Category> RenameGroupCommand { get; }
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

        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
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

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand<Product> DeleteProductCommand { get; }
        public DelegateCommand AddProductCommand { get; }

        public bool IsEnabledAddProduct => Category != null;
        #endregion

        public ShowProductViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _ea = ea;
            _regionManager = regionManager;
            IsAddPurchase = false;
            SelectedGroupCommand = new DelegateCommand<Category>(SelectedGroupChange);
            AddStorePopupRequest = new InteractionRequest<INotification>();
            AddGroupPopupRequest = new InteractionRequest<INotification>();
            RenameGroupPopupRequest = new InteractionRequest<INotification>();
            ShowPropertiesPopupRequest = new InteractionRequest<INotification>();
            AddStoreCommand = new DelegateCommand(AddStore);
            AddGroupCommand = new DelegateCommand<Category>(AddGroup);
            RenameGroupCommand = new DelegateCommand<Category>(RenameGroup);
            DeleteGroupCommand = new DelegateCommand<Category>(DeleteGroupAsync);
            ShowPropertiesCommand = new DelegateCommand<Category>(ShowProperties);
            _ea.GetEvent<IsReadySentEvent>().Subscribe(obj => _ea.GetEvent<BoolSentEvent>().Publish(IsAddPurchase));

            AddProductPopupRequest = new InteractionRequest<INotification>();
            DeleteProductCommand = new DelegateCommand<Product>(DeleteProductAsync);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabledAddProduct);

            AddCommandPurchaseGoods = new DelegateCommand<Product>(AddPurchaseGoods);
        }

        #region GroupCommands

        private async void LoadGroupAsync()
        {
            try
            {
                SqlCategoryRepository dbSet = new SqlCategoryRepository();
                CategoriesList = new ObservableCollection<Category>(await dbSet.GetListAsync(CategorySpecification.GetCategoriesByIdParent(), include: c => c.ChildCategories));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedGroupChange(Category obj)
        {
            Category = obj;
        }

        private void AddStore()
        {
            AddStorePopupRequest.Raise(new Confirmation { Title = "Добавить магазин", Content = null }, Callback);
        }

        private void AddGroup(Category category)
        {
            AddGroupPopupRequest.Raise(new Confirmation { Title = "Добавить группу", Content = category.Id }, Callback);
        }

        private void RenameGroup(Category category)
        {
            RenameGroupPopupRequest.Raise(new Confirmation { Title = "Переименовать группу", Content = category}, Callback);
        }

        private void ShowProperties(Category category)
        {
            ShowPropertiesPopupRequest.Raise(new Notification { Title = "Свойства", Content = category });
        }

        private async void DeleteGroupAsync(Category category)
        {
            try
            {
                if (category.IdParent == null)
                {
                    if (MessageBox.Show("Удалить магазин?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    SqlCategoryRepository dbSet = new SqlCategoryRepository();
                    await dbSet.DeleteAsync(category);
                    MessageBox.Show("Магазин удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGroupAsync();
                }
                else
                {
                    if (MessageBox.Show("Удалить группу?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    SqlCategoryRepository dbSet = new SqlCategoryRepository();
                    await dbSet.DeleteAsync(category);
                    MessageBox.Show("Группа удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGroupAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Callback(INotification notification)
        {
            if (!((Confirmation) notification).Confirmed) return;
            LoadGroupAsync();
            LoadProductAsync();
        }

        #endregion

        #region ProductCommands

        private void LoadProductAsync()
        {
            try
            {
                SqlUnitStorageRepository sqlUnitStorageRepository = new SqlUnitStorageRepository();
                SqlWarrantyPeriodRepository sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                SqlProductRepository sqlProductRepository = new SqlProductRepository();

                var loadUnitStorage = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());
                var loadProduct = Task.Run(() => sqlProductRepository.GetListAsync(Category == null
                    ? ProductSpecification.GetProductsByFindString(FindString)
                    : ProductSpecification.GetProductsByIdGroup(Category.Id)));

                Task.WaitAll(new Task[] { loadUnitStorage, loadWarrantyPeriod, loadProduct });

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                ProductsList = new ObservableCollection<Product>(loadProduct.Result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            AddProductPopupRequest.Raise(new Confirmation { Title = "Добавить товар", Content = Category}, Callback);
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
            LoadGroupAsync();
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

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using ModelModul.Product;
using ModelModul.UnitStorage;
using ModelModul.WarrantyPeriod;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : BindableBase, INavigationAware, IRegionMemberLifetime, IInteractionRequestAware
    {
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
                SelectedGroup = null;
                LoadGroupAsync();
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand<ProductViewModel> AddCommandPurchaseGoods { get; }

        #endregion

        #region GroupProperties

        private ObservableCollection<Groups> _groupsList = new ObservableCollection<Groups>();

        public ObservableCollection<Groups> GroupsList
        {
            get => _groupsList;
            set => SetProperty(ref _groupsList, value);
        }

        public DelegateCommand<Groups> SelectedGroupCommand { get; }

        public InteractionRequest<INotification> AddStorePopupRequest { get; set; }
        public InteractionRequest<INotification> AddGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> RenameGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> ShowPropertiesPopupRequest { get; set; }
        public DelegateCommand<Groups> DeleteGroupCommand { get; }
        public DelegateCommand AddStoreCommand { get; }
        public DelegateCommand<Groups> AddGroupCommand { get; }
        public DelegateCommand<Groups> RenameGroupCommand { get; }
        public DelegateCommand<Groups> ShowPropertiesCommand { get; }

        #endregion

        private ObservableCollection<WarrantyPeriods> _warrantyPeriods = new ObservableCollection<WarrantyPeriods>();
        public ObservableCollection<WarrantyPeriods> WarrantyPeriodsList
        {
            get => _warrantyPeriods;
            set => SetProperty(ref _warrantyPeriods, value);
        }

        private ObservableCollection<UnitStorages> _unitStorages = new ObservableCollection<UnitStorages>();
        public ObservableCollection<UnitStorages> UnitStoragesList
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
        }

        #region ProductProperties

        private Groups _selectedGroup;
        public Groups SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
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

        private ObservableCollection<ProductViewModel> _productsList = new ObservableCollection<ProductViewModel>();
        public ObservableCollection<ProductViewModel> ProductsList
        {
            get => _productsList;
            set => SetProperty(ref _productsList, value);
        }

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand<ProductViewModel> DeleteProductCommand { get; }
        public DelegateCommand AddProductCommand { get; }

        public bool IsEnabledAddProduct => SelectedGroup != null;
        #endregion

        public ShowProductViewModel()
        {
            LoadGroupAsync();
            IsAddPurchase = false;
            SelectedGroupCommand = new DelegateCommand<Groups>(SelectedGroupChange);
            AddStorePopupRequest = new InteractionRequest<INotification>();
            AddGroupPopupRequest = new InteractionRequest<INotification>();
            RenameGroupPopupRequest = new InteractionRequest<INotification>();
            ShowPropertiesPopupRequest = new InteractionRequest<INotification>();
            AddStoreCommand = new DelegateCommand(AddStore);
            AddGroupCommand = new DelegateCommand<Groups>(AddGroup);
            RenameGroupCommand = new DelegateCommand<Groups>(RenameGroup);
            DeleteGroupCommand = new DelegateCommand<Groups>(DeleteGroup);
            ShowPropertiesCommand = new DelegateCommand<Groups>(ShowProperties);


            AddProductPopupRequest = new InteractionRequest<INotification>();
            DeleteProductCommand = new DelegateCommand<ProductViewModel>(DeleteProduct);
            AddProductCommand = new DelegateCommand(AddProduct).ObservesCanExecute(() => IsEnabledAddProduct);

            AddCommandPurchaseGoods = new DelegateCommand<ProductViewModel>(AddPurchaseGoods);
        }

        #region GroupCommands

        private async Task LoadGroupAsync()
        {
            try
            {
                DbSetGroups dbSet = new DbSetGroups();
                GroupsList = await dbSet.LoadAsync();
                RaisePropertyChanged("GroupsList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedGroupChange(Groups obj)
        {
            SelectedGroup = obj;
        }

        private void AddStore()
        {
            AddStorePopupRequest.Raise(new Confirmation { Title = "Добавить магазин", Content = null }, Callback);
        }

        private void AddGroup(Groups group)
        {
            AddGroupPopupRequest.Raise(new Confirmation { Title = "Добавить группу", Content = @group.Id }, Callback);
        }

        private void RenameGroup(Groups group)
        {
            RenameGroupPopupRequest.Raise(new Confirmation { Title = "Переименовать группу", Content = group}, Callback);
        }

        private void ShowProperties(Groups group)
        {
            ShowPropertiesPopupRequest.Raise(new Notification { Title = "Свойства", Content = group });
        }

        private async void DeleteGroup(Groups group)
        {
            try
            {
                if (group.IdParentGroup == null)
                {
                    if (MessageBox.Show("Удалить магазин?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    DbSetGroups dbSet = new DbSetGroups();
                    await dbSet.DeleteAsync(group.Id);
                    MessageBox.Show("Магазин удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGroupAsync();
                }
                else
                {
                    if (MessageBox.Show("Удалить группу?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    DbSetGroups dbSet = new DbSetGroups();
                    await dbSet.DeleteAsync(group.Id);
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

        private async Task LoadProductAsync()
        {
            try
            {
                DbSetUnitStorages dbSetUnitStorages = new DbSetUnitStorages();
                UnitStoragesList = await dbSetUnitStorages.LoadAsync();
                DbSetWarrantyPeriods dbSetWarrantyPeriods = new DbSetWarrantyPeriods();
                WarrantyPeriodsList = await dbSetWarrantyPeriods.LoadAsync();
                DbSetProducts dbSet = new DbSetProducts();
                if (SelectedGroup == null)
                    ProductsList = new ObservableCollection<ProductViewModel>(
                        (await dbSet.LoadAsync(-1, FindString)).Select(objPr => new ProductViewModel
                        {
                            Product = objPr
                        }));
                else
                    ProductsList = new ObservableCollection<ProductViewModel>(
                        (await dbSet.LoadAsync(SelectedGroup.Id, FindString)).Select(objPr => new ProductViewModel
                        {
                            Product = objPr
                        }));
                RaisePropertyChanged("ProductsList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct()
        {
            AddProductPopupRequest.Raise(new Confirmation { Title = "Добавить товар", Content = SelectedGroup}, Callback);
        }

        private async void DeleteProduct(ProductViewModel obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить товар?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetProducts dbSet = new DbSetProducts();
                await dbSet.DeleteAsync(obj.Id);
                MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadProductAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Navigat

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            FindString = "";
            SelectedGroup = null;
            LoadGroupAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public bool KeepAlive => false;

        #endregion

        private void AddPurchaseGoods(ProductViewModel obj)
        {
            if (_notification != null)
            {
                _notification.Confirmed = true;
                _notification.Content = obj.Product;
            }
            FinishInteraction?.Invoke();
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using ModelModul.Product;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : BindableBase, INavigationAware, IRegionMemberLifetime, IInteractionRequestAware
    {
        private bool _isAddPurchase;
        public bool IsAddPurchase
        {
            get => _isAddPurchase;
            set => SetProperty(ref _isAddPurchase, value);
        }

        #region GroupProperties

        private readonly DbSetGroups _groupModel = new DbSetGroups();

        public ObservableCollection<Groups> GroupsList => _groupModel.List;

        public DelegateCommand<Groups> SelectedGroupCommand { get; }

        public InteractionRequest<INotification> AddStorePopupRequest { get; set; }
        public InteractionRequest<INotification> AddGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> UpdateStorePopupRequest { get; set; }
        public InteractionRequest<INotification> ShowPropertiesPopupRequest { get; set; }
        public DelegateCommand<Groups> DeleteGroupCommand { get; }
        public DelegateCommand AddStoreCommand { get; }
        public DelegateCommand<Groups> AddGroupCommand { get; }
        public DelegateCommand<Groups> UpdateGroupCommand { get; }
        public DelegateCommand<Groups> ShowPropertiesCommand { get; }

        #endregion

        #region ProductProperties

        private Groups _selectedGroup;
        public Groups SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                LoadProductAsync();
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

        private readonly DbSetProducts _productsModel = new DbSetProducts();

        public ObservableCollection<Products> ProductsList => _productsModel.List;

        public InteractionRequest<INotification> UpdateProductPopupRequest { get; set; }

        public DelegateCommand<Products> DeleteProductCommand { get; }
        public DelegateCommand<Products> UpdateProductCommand { get; }
        public DelegateCommand<object> AddCommandPurchaseGoods { get; }
       

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

        #endregion

        public ShowProductViewModel()
        {
            LoadGroupAsync();
            IsAddPurchase = false;
            SelectedGroupCommand = new DelegateCommand<Groups>(SelectedGroupChange);
            AddStorePopupRequest = new InteractionRequest<INotification>();
            AddGroupPopupRequest = new InteractionRequest<INotification>();
            UpdateStorePopupRequest = new InteractionRequest<INotification>();
            ShowPropertiesPopupRequest = new InteractionRequest<INotification>();
            AddStoreCommand = new DelegateCommand(AddStore);
            UpdateGroupCommand = new DelegateCommand<Groups>(UpdateGroup);
            DeleteGroupCommand = new DelegateCommand<Groups>(DeleteGroup);
            AddGroupCommand = new DelegateCommand<Groups>(AddGroup);
            ShowPropertiesCommand = new DelegateCommand<Groups>(ShowProperties);

            UpdateProductPopupRequest = new InteractionRequest<INotification>();
            AddCommandPurchaseGoods = new DelegateCommand<object>(AddPurchaseGoods);
            DeleteProductCommand = new DelegateCommand<Products>(DeleteProduct);
            UpdateProductCommand = new DelegateCommand<Products>(UpdateProduct);
        }
        
        #region GroupCommands

        private async Task LoadGroupAsync()
        {
            await _groupModel.LoadAsync();
            RaisePropertyChanged("GroupsList");
        }

        private void SelectedGroupChange(Groups obj)
        {
            SelectedGroup = obj;
        }

        private void AddStore()
        {
            AddStorePopupRequest.Raise(new Confirmation { Title = "Добавить магазин" }, Callback);
        }

        private void AddGroup(Groups group)
        {
            AddGroupPopupRequest.Raise(new Confirmation { Title = "Добавить группу", Content = group.Groups2 }, Callback);
        }

        private void UpdateGroup(Groups group)
        {
            if (group.IdParentGroup == null)
            {
                UpdateStorePopupRequest.Raise(new Confirmation { Title = "Изменить магазин", Content = group.Groups2 }, Callback);
            }
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
                    await _groupModel.DeleteAsync(group.Id);
                    MessageBox.Show("Магазин удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGroupAsync();
                }
                else
                {
                    if (MessageBox.Show("Удалить группу?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    await _groupModel.DeleteAsync(group.Id);
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
            if (SelectedGroup == null) await _productsModel.LoadAsync(-1, FindString);
            else await _productsModel.LoadAsync(SelectedGroup.Id, FindString);
            RaisePropertyChanged("ProductsList");
        }

        private void AddPurchaseGoods(object obj)
        {
            if (_notification != null)
            {
                _notification.Confirmed = true;
                _notification.Content = obj;
            }
            FinishInteraction?.Invoke();
        }

        private void UpdateProduct(Products obj)
        {
            UpdateProductPopupRequest.Raise(new Confirmation { Title = "Изменить товар", Content = obj}, Callback);
        }

        private async void DeleteProduct(Products obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить товар?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                await _productsModel.DeleteAsync(obj.Id);
                MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProductAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
    }
}

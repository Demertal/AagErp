using System;
using System.Collections.ObjectModel;
using System.Windows;
using EventAggregatorLibrary;
using ModelModul;
using ModelModul.Group;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace GroupModul.ViewModels
{
    class ShowGroupViewModel : BindableBase
    {
        #region Parametrs

        readonly IEventAggregator _ea;

        private DbSetGroupsModel _dbSetGroupsModel = new DbSetGroupsModel();
        public DbSetGroupsModel DbSetGroupsModel
        {
            get => _dbSetGroupsModel;
            set
            {
                SetProperty(ref _dbSetGroupsModel, value);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Groups> Groups => DbSetGroupsModel.List;

        public InteractionRequest<INotification> AddStorePopupRequest { get; set; }
        public InteractionRequest<INotification> AddGroupPopupRequest { get; set; }
        public InteractionRequest<INotification> UpdateStorePopupRequest { get; set; }

        public DelegateCommand<Groups> SelectedGroupCommand { get; }
        public DelegateCommand<Groups> DeleteGroupCommand { get; }
        public DelegateCommand AddStoreCommand { get; }
        public DelegateCommand<Groups> AddGroupCommand { get; }
        public DelegateCommand<Groups> UpdateGroupCommand { get; }
        public DelegateCommand <object> GroupСhangeCommand { get; }

        #endregion

        public ShowGroupViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Reload();
            SelectedGroupCommand = new DelegateCommand<Groups>(SendSelectedGroupId);
            AddStorePopupRequest = new InteractionRequest<INotification>();
            AddGroupPopupRequest = new InteractionRequest<INotification>();
            UpdateStorePopupRequest = new InteractionRequest<INotification>();
            AddStoreCommand = new DelegateCommand(AddStore);
            UpdateGroupCommand = new DelegateCommand<Groups>(UpdateGroup);
            DeleteGroupCommand = new DelegateCommand<Groups>(DeleteGroup);
            AddGroupCommand = new DelegateCommand<Groups>(AddGroup);
            GroupСhangeCommand = new DelegateCommand<object>(GroupСhange);
        }

        #region Load

        public async void Reload()
        {
            await DbSetGroupsModel.Load();
            RaisePropertyChanged("Groups");
        }

        #endregion

        #region Commands

        private void SendSelectedGroupId(Groups group)
        {
             _ea.GetEvent<GroupsEventAggregator>().Publish(group);
        }

        private void AddStore()
        {
            AddStorePopupRequest.Raise(new Confirmation{ Title = "Добавить магазин" }, Callback);
        }

        private void AddGroup(Groups group)
        {
            AddGroupPopupRequest.Raise(new Confirmation { Title = "Добавить группу", Content = group.Groups2}, Callback);
        }

        private void UpdateGroup(Groups group)
        {
            if (group.IdParentGroup == null)
            {
                UpdateStorePopupRequest.Raise(new Confirmation { Title = "Изменить магазин", Content = group.Groups2 }, Callback);
            }
        }

        private void GroupСhange(object obj)
        {

        }

        private void DeleteGroup(Groups group)
        {
            if (group.IdParentGroup == null)
            {
                if (MessageBox.Show("Удалить магазин?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        DbSetGroupsModel.Delete(group.IdParentGroup.Value);
                        MessageBox.Show("Магазин удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Reload();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        throw;
                    }
                }
            }
        }

        private void Callback(INotification notification)
        {
            if (((Confirmation)notification).Confirmed)
            {
                Reload();
            }
        }

        #endregion

    }
}

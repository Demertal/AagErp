using EventAggregatorLibrary;
using ModelModul.Group;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace GroupModul.ViewModels
{
    class ShowGroupViewModel : BindableBase
    {
        #region Parametrs

        readonly IEventAggregator _ea;

        private ListGroupsModel _listGroups = new ListGroupsModel();
        public ListGroupsModel ListGroups
        {
            get => _listGroups;
            set
            {
                SetProperty(ref _listGroups, value);
                RaisePropertyChanged();
            }
        }

        ListGroupsModel _editableGroup;
        public ListGroupsModel EditableGroup
        {
            get => _editableGroup;
            set => SetProperty(ref _editableGroup, value);
        }

        public ObservableCollection<ListGroupsModel> Groups => ListGroups.Groups;

        public InteractionRequest<INotification> AddStorePopupRequest { get; set; }
        public InteractionRequest<INotification> AddGroupPopupRequest { get; set; }

        public DelegateCommand<ListGroupsModel> SelectedGroupCommand { get; }
        public DelegateCommand<ListGroupsModel> DeleteGroupCommand { get; }
        public DelegateCommand AddStoreCommand { get; }
        public DelegateCommand<ListGroupsModel> AddGroupCommand { get; }
        public DelegateCommand<ListGroupsModel> RenameGroupCommand { get; }
        public DelegateCommand <object> GroupСhangeCommand { get; }

        #endregion

        public ShowGroupViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Reload();
            SelectedGroupCommand = new DelegateCommand<ListGroupsModel>(SendSelectedGroupId);
            AddStorePopupRequest = new InteractionRequest<INotification>();
            AddGroupPopupRequest = new InteractionRequest<INotification>();
            AddStoreCommand = new DelegateCommand(AddStore);
            RenameGroupCommand = new DelegateCommand<ListGroupsModel>(RenameGroup);
            DeleteGroupCommand = new DelegateCommand<ListGroupsModel>(DeleteGroup);
            AddGroupCommand = new DelegateCommand<ListGroupsModel>(AddGroup);
            GroupСhangeCommand = new DelegateCommand<object>(GroupСhange);
        }

        #region Load

        public async void Reload()
        {
            await ListGroups.Load();
            await LoadNode(ListGroups);
            RaisePropertyChanged("Groups");
        }

        public async Task<int> LoadNode(ListGroupsModel group)
        {
            foreach (var item in group.Groups)
            {
                await item.Load();
                await LoadNode(item);
            }
            return group.Groups.Count;
        }

        #endregion

        #region Commands

        private void SendSelectedGroupId(ListGroupsModel listGroupsModel)
        {
            EditableGroup = null;
            if (listGroupsModel?.ParentGroup == null) _ea.GetEvent<IntEventAggregator>().Publish(-1);
            else _ea.GetEvent<IntEventAggregator>().Publish(listGroupsModel.ParentGroup.Id);
        }

        private void AddStore()
        {
            AddStorePopupRequest.Raise(new Confirmation{ Title = "Добавить магазин" }, Callback);
        }

        private void AddGroup(ListGroupsModel obj)
        {
            AddGroupPopupRequest.Raise(new Confirmation { Title = "Добавить группу", Content = obj.ParentGroup}, Callback);
        }

        private void RenameGroup(ListGroupsModel listGroupsModel)
        {
            EditableGroup = listGroupsModel;
            //if (listGroupsModel.ParentGroup.IdParentGroup == null)
            //{
            //    UpdateStorePopupRequest.Raise(new Confirmation { Title = "Изменить магазин", Content = listGroupsModel.ParentGroup}, Callback);
            //}
        }

        private void GroupСhange(object obj)
        {

        }

        private void DeleteGroup(ListGroupsModel listGroupsModel)
        {
            if (listGroupsModel.ParentGroup.IdParentGroup == null)
            {
                if (MessageBox.Show("Удалить магазин?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    if (listGroupsModel.Delete(listGroupsModel.ParentGroup.Id))
                    {
                        MessageBox.Show("Магазин удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Reload();
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

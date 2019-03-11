using System;
using System.Windows;
using ModelModul.Group;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace GroupModul.ViewModels
{
    class UpdateStoreViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        private GroupModel _oldGroupModel;

        private string _title = "";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnabled");
            }
        }

        private bool IsEnabled => Title != "";

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                _oldGroupModel = (GroupModel)_notification.Content;
                Title = _oldGroupModel.Title;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand UpdateStoreCommand { get; }

        #endregion

        public UpdateStoreViewModel()
        {
            UpdateStoreCommand = new DelegateCommand(UpdateStore).ObservesCanExecute(() => IsEnabled);
        }

        public void UpdateStore()
        {
            try
            {
                if (_oldGroupModel.Title != Title)
                {
                    ListGroupsModel listGroups = new ListGroupsModel();
                    listGroups.Update(new GroupModel {Title = Title, Id = _oldGroupModel.Id});
                    MessageBox.Show("Магазин изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (_notification != null)
                        _notification.Confirmed = true;
                }
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
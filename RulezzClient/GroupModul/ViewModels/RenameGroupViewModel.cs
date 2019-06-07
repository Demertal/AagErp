using System;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace GroupModul.ViewModels
{
    class RenameGroupViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        private Groups _oldGroupModel;

        private string _title = "";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
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
                _oldGroupModel = (Groups)_notification.Content;
                Title = _oldGroupModel.Title;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand UpdateStoreCommand { get; }

        #endregion

        public RenameGroupViewModel()
        {
            UpdateStoreCommand = new DelegateCommand(UpdateStore).ObservesCanExecute(() => IsEnabled);
        }

        public async void UpdateStore()
        {
            string temp = _oldGroupModel.Title;
            try
            {
                if (_oldGroupModel.Title != Title)
                {
                    DbSetGroups dbSetGroups = new DbSetGroups();
                    _oldGroupModel.Title = Title;
                    await dbSetGroups.UpdateAsync(_oldGroupModel);
                    MessageBox.Show("Группа изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (_notification != null)
                        _notification.Confirmed = true;
                }
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                _oldGroupModel.Title = temp;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
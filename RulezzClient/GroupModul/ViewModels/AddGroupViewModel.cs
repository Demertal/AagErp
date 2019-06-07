using System;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace GroupModul.ViewModels
{
    class AddGroupViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        private readonly Groups _groupModel = new Groups();
        public string Title
        {
            get => _groupModel.Title;
            set
            {
                _groupModel.Title = value;
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
                if(_notification.Content != null)
                    _groupModel.IdParentGroup = (int?)_notification.Content;
                else
                    _groupModel.IdParentGroup = null;
                Title = "";
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddGroupCommand { get; }

        #endregion

        public AddGroupViewModel()
        {
            AddGroupCommand = new DelegateCommand(AddGroup).ObservesCanExecute(() => IsEnabled);
        }

        public async void AddGroup()
        {
            try
            {
                DbSetGroups dbSetGroups = new DbSetGroups();
                await dbSetGroups.AddAsync(_groupModel);
                MessageBox.Show(_groupModel.IdParentGroup == null ? "Магазин добавлен" : "Группа добавлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
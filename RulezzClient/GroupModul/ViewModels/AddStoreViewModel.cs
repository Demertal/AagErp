using System;
using System.Windows;
using ModelModul;
using ModelModul.Group;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace GroupModul.ViewModels
{
    class AddStoreViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

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
                Title = "";
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddStoreCommand { get; }

        #endregion

        public AddStoreViewModel()
        {
            AddStoreCommand = new DelegateCommand(AddStore).ObservesCanExecute(() => IsEnabled);
        }

        public void AddStore()
        {
            try
            {
                DbSetGroups dbSetGroups = new DbSetGroups();
                dbSetGroups.Add(new Groups{ Title = Title });
                MessageBox.Show("Магазин добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

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

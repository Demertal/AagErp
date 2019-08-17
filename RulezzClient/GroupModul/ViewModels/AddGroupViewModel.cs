using System;
using System.Windows;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace GroupModul.ViewModels
{
    class AddGroupViewModel : ViewModelBase, IInteractionRequestAware
    {
        #region Properties

        private readonly Category _category = new Category();
        public string Title
        {
            get => _category.Title;
            set
            {
                _category.Title = value;
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
                    _category.IdParent = (int?)_notification.Content;
                else
                    _category.IdParent = null;
                Title = "";
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddGroupCommand { get; }

        #endregion

        public AddGroupViewModel()
        {
            AddGroupCommand = new DelegateCommand(AddGroupAsync).ObservesCanExecute(() => IsEnabled);
        }

        public async void AddGroupAsync()
        {
            try
            {
                SqlCategoryRepository sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.CreateAsync(_category);
                MessageBox.Show(_category.IdParent == null ? "Магазин добавлен" : "Группа добавлена", "Успех",
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

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
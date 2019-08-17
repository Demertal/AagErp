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
    class RenameGroupViewModel : ViewModelBase, IInteractionRequestAware
    {
        #region Properties

        private Category _category;

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
                _category = (Category)_notification.Content;
                Title = _category.Title;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand UpdateStoreCommand { get; }

        #endregion

        public RenameGroupViewModel()
        {
            UpdateStoreCommand = new DelegateCommand(UpdateStore).ObservesCanExecute(() => IsEnabled);
        }

        public void UpdateStore()
        {
            string temp = _category.Title;
            try
            {
                if (_category.Title != Title)
                {
                    SqlCategoryRepository sqlCategoryRepository = new SqlCategoryRepository();
                    _category.Title = Title;
                    sqlCategoryRepository.UpdateAsync(_category);
                    MessageBox.Show("Группа изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (_notification != null)
                        _notification.Confirmed = true;
                }
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                _category.Title = temp;
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
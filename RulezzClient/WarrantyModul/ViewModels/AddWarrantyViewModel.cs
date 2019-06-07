using System;
using System.ComponentModel;
using System.Windows;
using ModelModul.Warranty;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace WarrantyModul.ViewModels
{
    class AddWarrantyViewModel : BindableBase, IInteractionRequestAware, INavigationAware
    {
        #region ProductProperties

        private WarrantyViewModel _warranty = new WarrantyViewModel();

        public WarrantyViewModel Warranty
        {
            get => _warranty;
            set => SetProperty(ref _warranty, value);
        }

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                _warranty.IdSerialNumber = (int) _notification.Content;
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddWarrantyCommand { get; }

        #endregion

        public AddWarrantyViewModel()
        {
            Warranty.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args) {RaisePropertyChanged(args.PropertyName);  };
            AddWarrantyCommand = new DelegateCommand(AddWarranty).ObservesCanExecute(() => Warranty.IsValidate);
        }

        public async void AddWarranty()
        {
            try
            {
                Warranty.DateDeparture = null;
                Warranty.DateIssue = null;
                Warranty.DateReceipt = null;
                DbSetWarranties dbSet = new DbSetWarranties();
                await dbSet.AddAsync(Warranty.Warranty);
                MessageBox.Show("Товар принят на гарантию", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
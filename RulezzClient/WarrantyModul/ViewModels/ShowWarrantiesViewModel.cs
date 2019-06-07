using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.SerialNumber;
using ModelModul.Warranty;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace WarrantyModul.ViewModels
{
    class ShowWarrantiesViewModel: BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        #region PropertiesSerialNumbers

        private string _findString;
        public string FindString
        {
            get => _findString;
            set
            {
                SetProperty(ref _findString, value);
                RaisePropertyChanged("IsEnabled");
                LoadSerialNumbersAsync();
            }
        }

        private ObservableCollection<SerialNumbers> _serialNumbersList = new ObservableCollection<SerialNumbers>();
        public ObservableCollection<SerialNumbers> SerialNumbersList
        {
            get => _serialNumbersList;
            set => SetProperty(ref _serialNumbersList, value);
        }

        private SerialNumbers _selectedSerialNumber;
        public SerialNumbers SelectedSerialNumber
        {
            get => _selectedSerialNumber;
            set
            {
                if(value != null)
                    SetProperty(ref _selectedSerialNumber, value);
                else
                    SetProperty(ref _selectedSerialNumber, new SerialNumbers());
                LoadWarrantiesAsync();
            }
        }
        #endregion

        #region PropertiesSerialNumbers

        private ObservableCollection<WarrantyViewModel> _warrantiesList = new ObservableCollection<WarrantyViewModel>();
        public ObservableCollection<WarrantyViewModel> WarrantiesList
        {
            get => _warrantiesList;
            set => SetProperty(ref _warrantiesList, value);
        }

        public InteractionRequest<INotification> AddWarrantyPopupRequest { get; set; }
        public DelegateCommand AddWarrantyCommand { get; }

        public bool IsEnabled => SelectedSerialNumber.SelleDate != null;

        #endregion

        public ShowWarrantiesViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            LoadSerialNumbersAsync();
            AddWarrantyPopupRequest = new InteractionRequest<INotification>();
            AddWarrantyCommand = new DelegateCommand(AddWarranty).ObservesCanExecute(() => IsEnabled);
        }

        private async void LoadSerialNumbersAsync()
        {
            try
            {
                DbSetSerialNumbers dbSet = new DbSetSerialNumbers();
                SerialNumbersList = await dbSet.LoadAsync(FindString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadWarrantiesAsync()
        {
            try
            {
                DbSetWarranties dbSet = new DbSetWarranties();
                WarrantiesList = new ObservableCollection<WarrantyViewModel>(
                    (await dbSet.LoadAsync(SelectedSerialNumber.Id)).Select(obj =>
                        new WarrantyViewModel {Warranty = obj}));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddWarranty()
        {
            AddWarrantyPopupRequest.Raise(new Confirmation { Title = "Принять товар по гарантии", Content = SelectedSerialNumber.Id }, Callback);
        }

        private void Callback(INotification obj)
        {
            if (!((Confirmation)obj).Confirmed) return;
            LoadWarrantiesAsync();
        }

        #region Navigat

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("WarrantyInfo");
        }

        #endregion
    }
}

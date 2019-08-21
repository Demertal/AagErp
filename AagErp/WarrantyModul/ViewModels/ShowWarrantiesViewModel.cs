using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace WarrantyModul.ViewModels
{
    class ShowWarrantiesViewModel: ViewModelBase
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

        private ObservableCollection<SerialNumber> _serialNumbersList = new ObservableCollection<SerialNumber>();
        public ObservableCollection<SerialNumber> SerialNumbersList
        {
            get => _serialNumbersList;
            set => SetProperty(ref _serialNumbersList, value);
        }

        private SerialNumber _selectedSerialNumber;
        public SerialNumber SelectedSerialNumber
        {
            get => _selectedSerialNumber;
            set
            {
                if(value != null)
                    SetProperty(ref _selectedSerialNumber, value);
                else
                    SetProperty(ref _selectedSerialNumber, new SerialNumber());
                LoadWarrantiesAsync();
            }
        }
        #endregion

        #region PropertiesSerialNumbers

        //private ObservableCollection<WarrantyViewModel> _warrantiesList = new ObservableCollection<WarrantyViewModel>();
        //public ObservableCollection<WarrantyViewModel> WarrantiesList
        //{
        //    get => _warrantiesList;
        //    set => SetProperty(ref _warrantiesList, value);
        //}

        public InteractionRequest<INotification> AddWarrantyPopupRequest { get; set; }
        public DelegateCommand AddWarrantyCommand { get; }

        public bool IsEnabled => true;// SelectedSerialNumber.SelleDate != null;

        #endregion

        public ShowWarrantiesViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddWarrantyPopupRequest = new InteractionRequest<INotification>();
            AddWarrantyCommand = new DelegateCommand(AddWarranty).ObservesCanExecute(() => IsEnabled);
        }

        private async void LoadSerialNumbersAsync()
        {
            try
            {
                SqlSerialNumberRepository sql = new SqlSerialNumberRepository();
                //SerialNumbersList = new ObservableCollection<SerialNumber>(await sql.GetListAsync(SerialNumberSpecification.GetSerialNumbersByContainsValue(FindString)));
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
                //SqlWarrantyRepository dbSet = new SqlWarrantyRepository();
                //WarrantiesList = new ObservableCollection<WarrantyViewModel>(
                //    (await dbSet.GetListAsync(WarrantySpecification.GetWarrantiesByIdSerialNumber(SelectedSerialNumber.Id))).Select(obj =>
                //        new WarrantyViewModel {Warranty = obj}));
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

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadSerialNumbersAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("WarrantyInfo");
        }

        #endregion
    }
}

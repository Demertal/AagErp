using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace ProductModul.ViewModels
{
    class AddProductViewModel : ViewModelBase, IInteractionRequestAware
    {
        #region ProductProperties

        private ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriods
        {
            get => _warrantyPeriods;
            set => SetProperty(ref _warrantyPeriods, value);
        }

        private ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStorages
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
        }

        //private ProductViewModel _product = new ProductViewModel();
        //public ProductViewModel Product
        //{
        //    get => _product;
        //    set
        //    {
        //        _product = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public string Group => Product?.Category == null ? "Группа:" : "Группа: " + Product.Category.Title;

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                //SetProperty(ref _notification, value as Confirmation);
                //Product = new ProductViewModel {Category = (Category) _notification.Content};
                //Product.IdGroup = Product.Category.Id;
                //UnitStorage tempUnit = UnitStorages.FirstOrDefault(obj => obj.Title == "шт");
                //Product.IdUnitStorage = tempUnit?.Id ?? 0;
                //WarrantyPeriod tempPeriod = WarrantyPeriods.FirstOrDefault(obj => obj.Period == "Нет");
                //Product.IdWarrantyPeriod = tempPeriod?.Id ?? 0;
                //RaisePropertyChanged("Parent");
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }

        #endregion

        public AddProductViewModel()
        {
            LoadAsync();
            //Product.PropertyChanged += (o, e) => RaisePropertyChanged(e.PropertyName);
            //AddProductCommand = new DelegateCommand(AddProductAsync).ObservesCanExecute(() => Product.IsValidate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
        }

        private async void LoadAsync()
        {
            try
            {
                SqlUnitStorageRepository sqlUnitStorageRepository = new SqlUnitStorageRepository();
                //UnitStorages = new ObservableCollection<UnitStorage>(await sqlUnitStorageRepository.GetListAsync());
                SqlWarrantyPeriodRepository sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                //WarrantyPeriods = new ObservableCollection<WarrantyPeriod>(await sqlWarrantyPeriodRepository.GetListAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void AddProductAsync()
        {
            //Category temp = _product.Category;
            //try
            //{
            //    _product.Category = null;
            //    _product.CountProducts = null;
            //    _product.PropertyProducts = null;
            //    _product.UnitStorage = null;
            //    _product.WarrantyPeriod = null;
            //    SqlProductRepository sqlProductRepository = new SqlProductRepository();
            //    await sqlProductRepository.CreateAsync((Product)_product.Product.Clone());
            //    MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            //    if (_notification != null)
            //        _notification.Confirmed = true;
            //    FinishInteraction?.Invoke();
            //}
            //catch (Exception ex)
            //{
            //    _product.Category = temp;
            //    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void GenerateBarcode()
        {
            try
            {
                SqlProductRepository sqlProductRepository = new SqlProductRepository();
                string temp;
                //do
                //{
                //    temp = GenerationBarcode.GenerateBarcode();
                //} while (sqlProductRepository.CheckBarcode(temp));

                //Product.Barcode = temp;
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
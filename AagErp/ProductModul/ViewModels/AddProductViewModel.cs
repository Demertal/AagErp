using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CustomControlLibrary.MVVM;
using GenerationBarcodeLibrary;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    public class AddProductViewModel : DialogViewModelBase
    {
        #region ProductProperties

        private ObservableCollection<WarrantyPeriod> _warrantyPeriodsListList = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriodsListList;
            set => SetProperty(ref _warrantyPeriodsListList, value);
        }

        private ObservableCollection<UnitStorage> _unitStoragesListList = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStoragesListList;
            set => SetProperty(ref _unitStoragesListList, value);
        }

        private ObservableCollection<PriceGroup> _priceGroupsListList = new ObservableCollection<PriceGroup>();
        public ObservableCollection<PriceGroup> PriceGroupsList
        {
            get => _priceGroupsListList;
            set => SetProperty(ref _priceGroupsListList, value);
        }

        private readonly Product _product = new Product();
        public Product Product => _product;

        private string _category;

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand GenerateBarcodeCommand { get; }

        #endregion

        public AddProductViewModel()
        {
            LoadAsync();
            Product.PropertyChanged += (o, e) => RaisePropertyChanged(e.PropertyName);
            AddProductCommand = new DelegateCommand(AddProductAsync).ObservesCanExecute(() => Product.IsValidate);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<UnitStorage> sqlUnitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> sqlWarrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                IRepository<PriceGroup> sqlPriceGroupRepository = new SqlPriceGroupRepository();

                var unitStoragesLoad = Task.Run(() => sqlUnitStorageRepository.GetListAsync());
                var warrantyPeriodsLoad = Task.Run(() => sqlWarrantyPeriodRepository.GetListAsync());
                var priceGroupsLoad = Task.Run(() => sqlPriceGroupRepository.GetListAsync());

                Task.WaitAll(unitStoragesLoad, warrantyPeriodsLoad, priceGroupsLoad);

                UnitStoragesList = new ObservableCollection<UnitStorage>(unitStoragesLoad.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(warrantyPeriodsLoad.Result);
                PriceGroupsList = new ObservableCollection<PriceGroup>(priceGroupsLoad.Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void AddProductAsync()
        {
            try
            {
                SqlProductRepository sqlProductRepository = new SqlProductRepository();
                await sqlProductRepository.CreateAsync(_product);
                MessageBox.Show("Товар добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "product", _product } }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GenerateBarcode()
        {
            try
            {
                IRepository<Product> sqlProductRepository = new SqlProductRepository();
                string temp;
                do
                {
                    temp = GenerationBarcode.GenerateBarcode();
                } while (await sqlProductRepository.AnyAsync(ProductSpecification.GetProductsByBarcode(temp)));

                Product.Barcode = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Category temp = parameters.GetValue<Category>("category");
                Category = temp.Title;
                Product.IdCategory = temp.Id;
                LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
            }
        }
    }
}
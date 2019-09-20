using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GenerationBarcodeLibrary;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using ModelModul.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : EntityViewModelBase<Product, ProductViewModel, SqlProductRepository>
    {
        #region Properties

        private ObservableCollection<WarrantyPeriod> _warrantyPeriodsList = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriodsList;
            set => SetProperty(ref _warrantyPeriodsList, value);
        }

        private ObservableCollection<UnitStorage> _unitStoragesList = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStoragesList;
            set => SetProperty(ref _unitStoragesList, value);
        }

        private ObservableCollection<PriceGroup> _priceGroupsListList = new ObservableCollection<PriceGroup>();
        public ObservableCollection<PriceGroup> PriceGroupsList
        {
            get => _priceGroupsListList;
            set => SetProperty(ref _priceGroupsListList, value);
        }

        private ObservableCollection<PropertyProduct> _propertyProductsList = new ObservableCollection<PropertyProduct>();
        public ObservableCollection<PropertyProduct> PropertyProductsList
        {
            get => _propertyProductsList;
            set => SetProperty(ref _propertyProductsList, value);
        }

        #endregion

        #region Command

        public DelegateCommand GenerateBarcodeCommand { get; }

        #endregion

        public ShowProductViewModel() : base("Товар добавлен", "Товар изменен")
        {
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
        }

        private async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                SqlProductRepository productForCountRepository = new SqlProductRepository();
                SqlProductRepository productForPropertyRepository = new SqlProductRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync(CancelTokenSource.Token),
                    CancelTokenSource.Token);
                var loadWarrantyPeriod = Task.Run(() => warrantyPeriodRepository.GetListAsync(CancelTokenSource.Token),
                    CancelTokenSource.Token);
                var priceGroupsLoad = Task.Run(() => priceGroupRepository.GetListAsync(CancelTokenSource.Token),
                    CancelTokenSource.Token);
                var loadCount =
                    Task.Run(() => productForCountRepository.GetCountsProduct(Entity.Id, CancelTokenSource.Token),
                        CancelTokenSource.Token);
                var loadProperty =
                    Task.Run(() => productForPropertyRepository.GetPropertyForProduct(Entity, CancelTokenSource.Token),
                        CancelTokenSource.Token);

                await Task.WhenAll(loadUnitStorage, loadWarrantyPeriod, priceGroupsLoad, loadCount, loadProperty);

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                PriceGroupsList = new ObservableCollection<PriceGroup>(priceGroupsLoad.Result);
                Entity.CountsProductCollection = new ObservableCollection<CountsProduct>(loadCount.Result);
                Entity.PropertyProductsCollection = new ObservableCollection<PropertyProduct>(loadProperty.Result);
                if(Entity.PriceProductsCollection != null)
                    foreach (var propertyProduct in Entity.PropertyProductsCollection)
                    {
                        propertyProduct.IdProduct = Entity.Id;
                    }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
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
                } while (await sqlProductRepository.AnyAsync(new CancellationToken(), ProductSpecification.GetProductsByBarcode(temp)));

                Entity.Barcode = temp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void PropertiesTransfer(Product fromEntity, Product toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
            toEntity.VendorCode = fromEntity.VendorCode;
            toEntity.Barcode = fromEntity.Barcode;
            toEntity.IdCategory = fromEntity.IdCategory;
            toEntity.Description = fromEntity.Description;
            toEntity.IdPriceGroup = fromEntity.IdPriceGroup;
            toEntity.IdUnitStorage = fromEntity.IdUnitStorage;
            toEntity.IdWarrantyPeriod = fromEntity.IdWarrantyPeriod;
            toEntity.KeepTrackSerialNumbers = fromEntity.KeepTrackSerialNumbers;
            toEntity.Category = fromEntity.Category;
            toEntity.PriceGroup = fromEntity.PriceGroup;
            toEntity.UnitStorage = fromEntity.UnitStorage;
            toEntity.Count = fromEntity.Count;
            toEntity.CountsProductCollection = fromEntity.CountsProductCollection;
            toEntity.Price = fromEntity.Price;
            toEntity.WarrantyPeriod = fromEntity.WarrantyPeriod;
            toEntity.PropertyProductsCollection = fromEntity.PropertyProductsCollection;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Backup = parameters.GetValue<Product>("entity");
                Entity = new ProductViewModel();
                if (Backup == null)
                {
                    Title = "Добавить";
                    Entity.Category = parameters.GetValue<Category>("category");
                    Entity.IdCategory = Entity.Category.Id;
                    IsAdd = true;
                }
                else
                {
                    Title = "Изменить";
                    IsAdd = false;
                    PropertiesTransfer(Backup, Entity);
                    LoadAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
            }
        }
    }
}

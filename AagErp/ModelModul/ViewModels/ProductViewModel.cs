using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class ProductViewModel : Product
    {
        #region Properties

        private CancellationTokenSource _cancelTokenTitle;
        private CancellationTokenSource _cancelTokenBarcode;

        private string _title;
        public override string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                ValidateTitle();
            }
        }

        private string _barcode;
        public override string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged("Barcode");
                ValidateBarcode();
            }
        }

        #endregion

        public ProductViewModel() { }

        public ProductViewModel(Product obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            VendorCode = obj.VendorCode;
            Barcode = obj.Barcode;
            IdCategory = obj.IdCategory;
            Description = obj.Description;
            IdPriceGroup = obj.IdPriceGroup;
            IdUnitStorage = obj.IdUnitStorage;
            IdWarrantyPeriod = obj.IdWarrantyPeriod;
            KeepTrackSerialNumbers = obj.KeepTrackSerialNumbers;
            Category = obj.Category;
            PriceGroup = obj.PriceGroup;
            UnitStorage = obj.UnitStorage;
            Count = obj.Count;
            CountsProductCollection = obj.CountsProductCollection;
            Price = obj.Price;
            WarrantyPeriod = obj.WarrantyPeriod;
            PropertyProductsCollection = obj.PropertyProductsCollection;
        }

        private async void ValidateTitle()
        {
            _cancelTokenTitle?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenTitle = newCts;

            const string propertyKey = "Title";

            try
            {
                IRepository<Product> productRepository = new SqlProductRepository();
                if (await productRepository.AnyAsync(_cancelTokenTitle.Token,
                    new ExpressionSpecification<Product>(o => o.Title == Title && o.Id != Id))
                )
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Товар с таким наименованием уже есть" };
                }
                else if (ValidationErrors.ContainsKey(propertyKey))
                {
                    ValidationErrors.Remove(propertyKey);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RaiseErrorsChanged(propertyKey);
            OnPropertyChanged("IsValid");

            if (_cancelTokenTitle == newCts)
                _cancelTokenTitle = null;
        }

        private async void ValidateBarcode()
        {
            _cancelTokenBarcode?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenBarcode = newCts;

            const string propertyKey = "Barcode";

            try
            {
                IRepository<Product> productRepository = new SqlProductRepository();
                if (await productRepository.AnyAsync(_cancelTokenBarcode.Token,
                    new ExpressionSpecification<Product>(o => o.Barcode == Barcode && o.Id != Id))
                )
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Товар с таким штрих-кодом уже есть" };
                }
                else if (ValidationErrors.ContainsKey(propertyKey))
                {
                    ValidationErrors.Remove(propertyKey);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RaiseErrorsChanged(propertyKey);
            OnPropertyChanged("IsValid");

            if (_cancelTokenBarcode == newCts)
                _cancelTokenBarcode = null;
        }
    }
}
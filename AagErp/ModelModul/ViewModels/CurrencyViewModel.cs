using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class CurrencyViewModel: Currency
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private string _title;
        public override string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                ValidateTitle();
            }
        }

        private bool _isDefault;
        public override bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                if (_isDefault) Cost = 1;
                OnPropertyChanged();
            }
        }

        #endregion

        public CurrencyViewModel() { }

        public CurrencyViewModel(Currency obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IsDefault = obj.IsDefault;
            Cost = obj.Cost;
        }

        private async void ValidateTitle()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Title";

            try
            {
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                if (await currencyRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<Currency>(c => c.Title == Title && c.Id != Id))) 
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Валюта с таким названием уже есть" };
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
            OnPropertyChanged(nameof(IsValid));

            if (_cancelTokenSource == newCts)
                _cancelTokenSource = null;
        }
    }
}

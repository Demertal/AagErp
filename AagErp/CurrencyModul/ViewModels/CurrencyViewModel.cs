using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace CurrencyModul.ViewModels
{
    public class CurrencyViewModel: Currency, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

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

        private bool _isDefault;
        public override bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                if (_isDefault) Cost = 1;
                OnPropertyChanged("IsDefault");
            }
        }

        public CurrencyViewModel() { }

        public CurrencyViewModel(Currency obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IsDefault = obj.IsDefault;
            Cost = obj.Cost;
        }

        #region INotifyDataErrorInfo members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors => _validationErrors.Count > 0;

        #endregion

        private async void ValidateTitle()
        {
            const string propertyKey = "Title";
            IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
            if (await currencyRepository.AnyAsync(new ExpressionSpecification<Currency>(c => c.Title == Title && c.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Валюта с таким названием уже есть" };
                RaiseErrorsChanged(propertyKey);
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
        }
    }
}

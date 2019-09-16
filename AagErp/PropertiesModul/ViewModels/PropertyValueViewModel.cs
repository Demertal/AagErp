using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace PropertyModul.ViewModels
{
    public class PropertyValueViewModel : PropertyValue, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
                ValidateValue();
            }
        }

        public PropertyValueViewModel() { }

        public PropertyValueViewModel(PropertyValue obj)
        {
            Id = obj.Id;
            Value = obj.Value;
            IdPropertyName = obj.IdPropertyName;
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

        private async void ValidateValue()
        {
            const string propertyKey = "Value";
            IRepository<PropertyValue> propertyValueRepository = new SqlPropertyValueRepository();
            if (await propertyValueRepository.AnyAsync(new ExpressionSpecification<PropertyValue>(w => w.Value == Value && w.IdPropertyName == IdPropertyName && w.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Такое значение уже есть" };
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
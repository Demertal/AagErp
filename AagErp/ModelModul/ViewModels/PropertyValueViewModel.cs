using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class PropertyValueViewModel : PropertyValue
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
                ValidateValue();
            }
        }

        #endregion

        public PropertyValueViewModel() { }

        public PropertyValueViewModel(PropertyValue obj)
        {
            Id = obj.Id;
            Value = obj.Value;
            IdPropertyName = obj.IdPropertyName;
        }

        private async void ValidateValue()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Value";

            try
            {
                IRepository<PropertyValue> propertyValueRepository = new SqlPropertyValueRepository();
                if (await propertyValueRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<PropertyValue>(w =>
                        w.Value == Value && w.IdPropertyName == IdPropertyName && w.Id != Id))) 
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Такое значение уже есть" };
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
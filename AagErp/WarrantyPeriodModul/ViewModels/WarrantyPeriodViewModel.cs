using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace WarrantyPeriodModul.ViewModels
{
    public class WarrantyPeriodViewModel : WarrantyPeriod, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private string _period;
        public override string Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged("Period");
                ValidatePeriod();
            }
        }

        public WarrantyPeriodViewModel() { }

        public WarrantyPeriodViewModel(WarrantyPeriod obj)
        {
            Id = obj.Id;
            Period = obj.Period;
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

        private async void ValidatePeriod()
        {
            const string propertyKey = "Period";
            IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();
            if (await warrantyPeriodRepository.AnyAsync(new ExpressionSpecification<WarrantyPeriod>(w => w.Period == Period && w.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Такой период уже есть" };
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
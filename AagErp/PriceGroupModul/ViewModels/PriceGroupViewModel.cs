using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace PriceGroupModul.ViewModels
{
    public class PriceGroupViewModel : PriceGroup, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private decimal _markup;
        public override decimal Markup
        {
            get => _markup;
            set
            {
                _markup = value;
                OnPropertyChanged("Markup");
                ValidateMarkup();
            }
        }

        public PriceGroupViewModel() { }

        public PriceGroupViewModel(PriceGroup obj)
        {
            Id = obj.Id;
            Markup = obj.Markup;
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

        private async void ValidateMarkup()
        {
            const string propertyKey = "Markup";
            IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
            if (await priceGroupRepository.AnyAsync(new ExpressionSpecification<PriceGroup>(w => w.Markup == Markup && w.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Такая наценка уже есть" };
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

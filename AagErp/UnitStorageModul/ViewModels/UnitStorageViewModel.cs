using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace UnitStorageModul.ViewModels
{
    public class UnitStorageViewModel : UnitStorage, INotifyDataErrorInfo
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

        public UnitStorageViewModel() { }

        public UnitStorageViewModel(UnitStorage obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IsWeightGoods = obj.IsWeightGoods;
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
            IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
            if (await unitStorageRepository.AnyAsync(new ExpressionSpecification<UnitStorage>(w => w.Title == Title && w.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Такая единица хранения уже есть" };
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

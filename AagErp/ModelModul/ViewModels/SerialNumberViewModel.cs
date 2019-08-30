using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ModelModul.Models;
using ModelModul.Repositories;

namespace ModelModul.ViewModels
{
    public class SerialNumberViewModel : SerialNumber, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private int? _idStore;
        public int? IdStore
        {
            get => _idStore;
            set
            {
                _idStore = value;
                ValidateValue();
            }
        }

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
            IRepository<SerialNumber> serialNumberRepository = new SqlSerialNumberRepository();
            if(IdStore != null)
            {
                int freeSerialNumbers =
                (await ((SqlSerialNumberRepository) serialNumberRepository).GetFreeSerialNumbers(IdProduct, Value,
                    IdStore.Value)).Count;
                if (freeSerialNumbers != 0 && Product != null)
                {
                    if (freeSerialNumbers - Product.SerialNumbers.Count(s => s.Value == Value) < 0)
                    {
                        _validationErrors[propertyKey] = new List<string> {"Нет доступных серийных номеров"};
                        RaiseErrorsChanged(propertyKey);
                    }
                    else
                    {
                        if (!_validationErrors.ContainsKey(propertyKey)) return;
                        _validationErrors.Remove(propertyKey);
                        RaiseErrorsChanged(propertyKey);
                    }
                }
                else if (freeSerialNumbers == 0)
                {
                    _validationErrors[propertyKey] = new List<string> {"Серийный номер не найден"};
                    RaiseErrorsChanged(propertyKey);
                }
                else if (_validationErrors.ContainsKey(propertyKey))
                {
                    _validationErrors.Remove(propertyKey);
                    RaiseErrorsChanged(propertyKey);
                }
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
        }
    }
}

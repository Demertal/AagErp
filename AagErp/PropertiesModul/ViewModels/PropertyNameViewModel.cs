using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;

namespace PropertyModul.ViewModels
{
    public class PropertyNameViewModel : PropertyName, INotifyDataErrorInfo
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

        public PropertyNameViewModel() { }

        public PropertyNameViewModel(PropertyName obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IdCategory = obj.IdCategory;
            Category = (Category)obj.Category?.Clone();
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
            SqlPropertyNameRepository propertyNameRepository = new SqlPropertyNameRepository();
            if (!await propertyNameRepository.CheckProperty(Id, IdCategory, Title))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Параметр с таким названием уже есть в этой категории" };
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

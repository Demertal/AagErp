using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace PropertyModul.ViewModels
{
    public class ShowPropertyViewModel: DialogViewModelBase, INotifyDataErrorInfo
    {
        #region Properties

        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private PropertyName _oldPropertyName;

        private PropertyName _propertyName;

        public PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                SetProperty(ref _propertyName, value);
                if (PropertyName != null)
                    PropertyName.PropertyChanged += (o, e) => { RaisePropertyChanged("PropertyName"); };
                RaisePropertyChanged("Name");
                RaisePropertyChanged("PropertyValuesCollection");
                RaisePropertyChanged("IsValidate");
            }
        }

        public string Name
        {
            get => PropertyName.Title;
            set
            {
                PropertyName.Title = value;
                RaisePropertyChanged("Name");
                RaisePropertyChanged("IsValidate");
                ValidateName();
            }
        }

        public ObservableCollection<PropertyValue> PropertyValuesCollection
        {
            get => PropertyName.PropertyValuesCollection as ObservableCollection<PropertyValue>;
            set
            {
                if (PropertyValuesCollection != null)
                    PropertyValuesCollection.CollectionChanged -= PropertyValuesCollectionChanged;
                PropertyName.PropertyValuesCollection = value;
                if (PropertyValuesCollection != null)
                    PropertyValuesCollection.CollectionChanged += PropertyValuesCollectionChanged;
                RaisePropertyChanged("PropertyValuesCollection");
            }
        }

        private bool _isValidateProperty, _isValidateName;
        public bool IsValidate => _isValidateProperty && _isValidateName;

        public DelegateCommand AcceptPropertyNameCommand { get; }
        public DelegateCommand<PropertyValue> DeletePropertyValueCommand { get; }

        #endregion

        public ShowPropertyViewModel()
        {
            _isValidateProperty = _isValidateName = true;
            PropertyName = new PropertyName();
            AcceptPropertyNameCommand = new DelegateCommand(AcceptPropertyNameAsync).ObservesCanExecute(() => IsValidate);
            DeletePropertyValueCommand = new DelegateCommand<PropertyValue>(DeletePropertyValue);
        }

        private void PropertyValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (PropertyValue item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= PropertyValueItemChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (PropertyValue item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += PropertyValueItemChanged;
                    }

                    break;
            }

            RaisePropertyChanged("PropertyValuesCollection");
        }

        private void PropertyValueItemChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidatePropertyValues((PropertyValue)sender);
            RaisePropertyChanged("PropertyValuesCollection");
        }

        private void DeletePropertyValue(PropertyValue obj)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить параметр?", "Удаление", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                PropertyValuesCollection.Remove(obj);
        }

        private async void LoadAsync()
        {
            try
            {
                IRepository<PropertyValue> propertyValueRepository = new SqlPropertyValueRepository();
                PropertyValuesCollection = new ObservableCollection<PropertyValue>(
                    await propertyValueRepository.GetListAsync(
                        PropertyValueSpecification.GetPropertyValuesByIdPropertyName(PropertyName.Id)));
                foreach (var propertyValue in PropertyValuesCollection)
                {
                    propertyValue.PropertyChanged += PropertyValueItemChanged;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void AcceptPropertyNameAsync()
        {
            try
            {
                string message;
                PropertyName temp = (PropertyName)PropertyName.Clone();
                IRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                if(temp.Id == 0)
                {
                    await propertyNameRepository.CreateAsync(temp);
                    message = "Параметр добавлен";
                }
                else
                {
                    await propertyNameRepository.UpdateAsync(temp);
                    message = "Параметр изменен";
                }
                MessageBox.Show(message, "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                if (_oldPropertyName != null)
                {
                    _oldPropertyName.Title = temp.Title;
                }
                _propertyName.Id = temp.Id;
                RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "property", _propertyName } }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Title = "Изменить параметр";
                _oldPropertyName = parameters.GetValue<PropertyName>("property");
                _propertyName = (PropertyName)_oldPropertyName?.Clone();
                if (_propertyName == null)
                {
                    Title = "Добавить параметр";
                    PropertyName = new PropertyName { Category = parameters.GetValue<Category>("category")};
                    PropertyName.IdCategory = PropertyName.Category?.Id;
                }
                else
                {
                    LoadAsync();
                }

                ValidateName();
                RaisePropertyChanged("PropertyName");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("PropertyValuesCollection");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
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

        private async void ValidateName()
        {
            _isValidateName = true;
            const string propertyKey = "Name";
            SqlPropertyNameRepository propertyNameRepository = new SqlPropertyNameRepository();
            if (string.IsNullOrEmpty(Name))
            {
                _isValidateName = false;
                _validationErrors[propertyKey] = new List<string> { "Наименование должно быть указано" };
                RaiseErrorsChanged(propertyKey);
            }
            else if (!await propertyNameRepository.CheckProperty(PropertyName.Id, PropertyName.IdCategory,
                PropertyName.Title))
            {
                _isValidateName = false;
                _validationErrors[propertyKey] =
                    new List<string> {"Параметр с таким названием уже есть в этой категории"};
                RaiseErrorsChanged(propertyKey);
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
            RaisePropertyChanged("IsValidate");
        }

        private void ValidatePropertyValues(PropertyValue obj)
        {
            _isValidateProperty = true;
            const string propertyKey = "PropertyValuesCollection";
            if (PropertyValuesCollection.Count(p => p.Value == obj.Value) > 1)
            {
                _isValidateProperty = false;
                _validationErrors[propertyKey] = new List<string> { "Параметр с таким названием уже есть в этой категории" };
                RaiseErrorsChanged(propertyKey);
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
            RaisePropertyChanged("IsValidate");
        }
    }
}

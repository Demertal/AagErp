using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace PropertiesModul.ViewModels
{
    class ShowPropertiesViewModel : ViewModelBase, IInteractionRequestAware
    {
        #region Properties

        private ObservableCollection<PropertyName> _propertyNames = new ObservableCollection<PropertyName>();
        public ObservableCollection<PropertyName> PropertyNamesList
        {
            get => _propertyNames;
            set => SetProperty(ref _propertyNames, value);
        }

        private ObservableCollection<PropertyValue> _propertyValues = new ObservableCollection<PropertyValue>();
        public ObservableCollection<PropertyValue> PropertyValuesList
        {
            get => _propertyValues;
            set => SetProperty(ref _propertyValues, value);
        }

        private Category _category = new Category();
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                RaisePropertyChanged();
            }
        }

        private PropertyName _propertyName = new PropertyName();
        public PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanUserAddValues");
                LoadValuesAsync();
            }
        }

        private Notification _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Notification);
                Category = (Category)_notification.Content;
                PropertyName = _propertyName;
                LoadNamesAsync();
            }
        }

        public bool CanUserAddValues => PropertyName != null && PropertyName.Id != 0;

        public Action FinishInteraction { get; set; }

        public DelegateCommand<SelectedCellsChangedEventArgs> SelectedPropertyNamesCommand { get; }

        public DelegateCommand<PropertyName> DeletePropertyNamesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyNamesCommand { get; }

        public DelegateCommand<PropertyValue> DeletePropertyValuesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyValuesCommand { get; }

        #endregion

        public ShowPropertiesViewModel()
        {
            SelectedPropertyNamesCommand = new DelegateCommand<SelectedCellsChangedEventArgs>(SelectedPropertyNames);
            ChangePropertyNamesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyNames);
            DeletePropertyNamesCommand = new DelegateCommand<PropertyName>(DeletePropertyNamesAsync);
            ChangePropertyValuesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyValues);
            DeletePropertyValuesCommand = new DelegateCommand<PropertyValue>(DeletePropertyValuesAsync);
        }

        private void SelectedPropertyNames(SelectedCellsChangedEventArgs obj)
        {
            PropertyName = obj.AddedCells.FirstOrDefault().Item as PropertyName ?? null;
        }

        #region PropertyName

        private void ChangePropertyNames(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((PropertyName)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddPropertyNamesAsync((PropertyName)obj.Row.Item);
                }
                else
                {
                    UpdatePropertyNamesAsync((PropertyName)obj.Row.Item);
                }
            }
        }

        private async void AddPropertyNamesAsync(PropertyName obj)
        {
            try
            {
                obj.IdCategory = Category.Id;
                SqlPropertyNameRepository sql = new SqlPropertyNameRepository();
                await sql.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNamesAsync();
        }

        private async void UpdatePropertyNamesAsync(PropertyName obj)
        {
            try
            {
                SqlPropertyNameRepository sql = new SqlPropertyNameRepository();
                await sql.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNamesAsync();
        }

        private async void DeletePropertyNamesAsync(PropertyName obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить параметр? Внимание, если у какого-то товара был установлен этот параметр он лишится его", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlPropertyNameRepository sql = new SqlPropertyNameRepository();
                await sql.DeleteAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNamesAsync();
        }

        private async void LoadNamesAsync()
        {
            try
            {
                SqlPropertyNameRepository sql = new SqlPropertyNameRepository();
                //PropertyNamesList = new ObservableCollection<PropertyName>(await sql.GetListAsync(PropertyNameSpecification.GetPropertyNamesByIdGroup(_category.Id)));
                if (PropertyName == null || PropertyName.Id == 0)
                {
                    PropertyName = PropertyNamesList.FirstOrDefault();
                }
                else
                {
                    if (PropertyNamesList.FirstOrDefault(objPr => objPr.Id == PropertyName.Id) == null)
                    {
                        PropertyName = PropertyNamesList.FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region PropertyValue

        private void ChangePropertyValues(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((PropertyValue)obj.Row.DataContext).Value))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddPropertyValuesAsync((PropertyValue)obj.Row.Item);
                }
                else
                {
                    UpdatePropertyValues((PropertyValue)obj.Row.Item);
                }
            }
        }

        private async void AddPropertyValuesAsync(PropertyValue obj)
        {
            try
            {
                obj.IdPropertyName = PropertyName.Id;
                SqlPropertyValueRepository sql = new SqlPropertyValueRepository();
                await sql.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValuesAsync();
        }

        private void UpdatePropertyValues(PropertyValue obj)
        {
            try
            {
                SqlPropertyValueRepository sql = new SqlPropertyValueRepository();
                sql.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValuesAsync();
        }

        private async void DeletePropertyValuesAsync(PropertyValue obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить значение параметра? Внимание, если у какого-то товара был установлено этот значение он лишится его", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlPropertyValueRepository sql = new SqlPropertyValueRepository();
                await sql.DeleteAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValuesAsync();
        }

        private async void LoadValuesAsync()
        {
            try
            {
                SqlPropertyValueRepository sql = new SqlPropertyValueRepository();
                //PropertyValuesList = PropertyName != null
                //    ? new ObservableCollection<PropertyValue>(await sql.GetListAsync(PropertyValueSpecification.GetPropertyValuesByIdPropertyName(PropertyName.Id)))
                //    : new ObservableCollection<PropertyValue>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadNamesAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}

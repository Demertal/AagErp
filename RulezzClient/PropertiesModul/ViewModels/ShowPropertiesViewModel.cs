using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModelModul;
using ModelModul.PropertyName;
using ModelModul.PropertyValue;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace PropertiesModul.ViewModels
{
    class ShowPropertiesViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        private ObservableCollection<PropertyNames> _propertyNames = new ObservableCollection<PropertyNames>();
        public ObservableCollection<PropertyNames> PropertyNamesList
        {
            get => _propertyNames;
            set => SetProperty(ref _propertyNames, value);
        }

        private ObservableCollection<PropertyValues> _propertyValues = new ObservableCollection<PropertyValues>();
        public ObservableCollection<PropertyValues> PropertyValuesList
        {
            get => _propertyValues;
            set => SetProperty(ref _propertyValues, value);
        }

        private Groups _group = new Groups();
        public Groups Group
        {
            get => _group;
            set
            {
                _group = value;
                RaisePropertyChanged();
            }
        }

        private PropertyNames _propertyName = new PropertyNames();
        public PropertyNames PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanUserAddValues");
                LoadValues();
            }
        }

        private Notification _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Notification);
                Group = (Groups)_notification.Content;
                PropertyName = _propertyName;
                LoadNames();
            }
        }

        public bool CanUserAddValues => PropertyName != null && PropertyName.Id != 0;

        public Action FinishInteraction { get; set; }

        public DelegateCommand<SelectedCellsChangedEventArgs> SelectedPropertyNamesCommand { get; }

        public DelegateCommand<PropertyNames> DeletePropertyNamesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyNamesCommand { get; }

        public DelegateCommand<PropertyValues> DeletePropertyValuesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyValuesCommand { get; }

        #endregion

        public ShowPropertiesViewModel()
        {
            LoadNames();
            SelectedPropertyNamesCommand = new DelegateCommand<SelectedCellsChangedEventArgs>(SelectedPropertyNames);
            ChangePropertyNamesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyNames);
            DeletePropertyNamesCommand = new DelegateCommand<PropertyNames>(DeletePropertyNames);
            ChangePropertyValuesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyValues);
            DeletePropertyValuesCommand = new DelegateCommand<PropertyValues>(DeletePropertyValues);
        }

        private void SelectedPropertyNames(SelectedCellsChangedEventArgs obj)
        {
            PropertyName = obj.AddedCells.FirstOrDefault().Item as PropertyNames ?? null;
        }

        #region PropertyNames

        private void ChangePropertyNames(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((PropertyNames)obj.Row.DataContext).Title))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddPropertyNames((PropertyNames)obj.Row.Item);
                }
                else
                {
                    UpdatePropertyNames((PropertyNames)obj.Row.Item);
                }
            }
        }

        private async void AddPropertyNames(PropertyNames obj)
        {
            try
            {
                obj.IdGroup = Group.Id;
                DbSetPropertyNames dbSet = new DbSetPropertyNames();
                await dbSet.AddAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNames();
        }

        private async void UpdatePropertyNames(PropertyNames obj)
        {
            try
            {
                DbSetPropertyNames dbSet = new DbSetPropertyNames();
                await dbSet.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNames();
        }

        private async void DeletePropertyNames(PropertyNames obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить параметр? Внимание, если у какого-то товара был установлен этот параметр он лишится его", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetPropertyNames dbSet = new DbSetPropertyNames();
                await dbSet.DeleteAsync(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadNames();
        }

        private async void LoadNames()
        {
            try
            {
                DbSetPropertyNames dbSet = new DbSetPropertyNames();
                PropertyNamesList = new ObservableCollection<PropertyNames>(await dbSet.LoadAsync(_group.Id));
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

        #region PropertyValues

        private void ChangePropertyValues(DataGridCellEditEndingEventArgs obj)
        {
            if (obj == null) return;
            if (string.IsNullOrEmpty(((PropertyValues)obj.Row.DataContext).Value))
            {
                obj.Cancel = true;
            }
            else
            {
                if (obj.Row.IsNewItem)
                {
                    AddPropertyValues((PropertyValues)obj.Row.Item);
                }
                else
                {
                    UpdatePropertyValues((PropertyValues)obj.Row.Item);
                }
            }
        }

        private async void AddPropertyValues(PropertyValues obj)
        {
            try
            {
                obj.IdPropertyName = PropertyName.Id;
                DbSetPropertyValue dbSet = new DbSetPropertyValue();
                await dbSet.AddAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValues();
        }

        private async void UpdatePropertyValues(PropertyValues obj)
        {
            try
            {
                DbSetPropertyValue dbSet = new DbSetPropertyValue();
                await dbSet.UpdateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValues();
        }

        private async void DeletePropertyValues(PropertyValues obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить значение параметра? Внимание, если у какого-то товара был установлено этот значение он лишится его", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                DbSetPropertyValue dbSet = new DbSetPropertyValue();
                await dbSet.DeleteAsync(obj.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadValues();
        }

        private async void LoadValues()
        {
            try
            {
                DbSetPropertyValue dbSet = new DbSetPropertyValue();
                PropertyValuesList = PropertyName != null
                    ? new ObservableCollection<PropertyValues>(await dbSet.LoadAsync(PropertyName.Id))
                    : new ObservableCollection<PropertyValues>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}

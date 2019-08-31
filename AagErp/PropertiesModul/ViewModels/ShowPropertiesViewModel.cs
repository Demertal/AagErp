using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace PropertyModul.ViewModels
{
    public class ShowPropertiesViewModel : DialogViewModelBase
    {
        #region Properties

        private ObservableCollection<PropertyName> _propertyNamesList = new ObservableCollection<PropertyName>();
        public ObservableCollection<PropertyName> PropertyNamesList
        {
            get => _propertyNamesList;
            set => SetProperty(ref _propertyNamesList, value);
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

        //private Notification _notification;
        //public INotification Notification
        //{
        //    get => _notification;
        //    set
        //    {
        //        SetProperty(ref _notification, value as Notification);
        //        Category = (Category)_notification.Content;
        //        PropertyName = _propertyName;
        //        LoadNamesAsync();
        //    }
        //}

        public DelegateCommand<SelectedCellsChangedEventArgs> SelectedPropertyNamesCommand { get; }

        public DelegateCommand<PropertyName> DeletePropertyNamesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyNamesCommand { get; }

        public DelegateCommand<PropertyValue> DeletePropertyValuesCommand { get; }
        public DelegateCommand<DataGridCellEditEndingEventArgs> ChangePropertyValuesCommand { get; }

        #endregion

        public ShowPropertiesViewModel()
        {
            ChangePropertyNamesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyNames);
            DeletePropertyNamesCommand = new DelegateCommand<PropertyName>(DeletePropertyNamesAsync);
            ChangePropertyValuesCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(ChangePropertyValues);
            DeletePropertyValuesCommand = new DelegateCommand<PropertyValue>(DeletePropertyValuesAsync);
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
                IRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                PropertyNamesList = new ObservableCollection<PropertyName>(
                    await propertyNameRepository.GetListAsync(
                        PropertyNameSpecification.GetPropertyNamesByIdGroup(_category.Id),
                        include: p => p.PropertyValuesCollection));
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
                //obj.IdPropertyName = PropertyName.Id;
                SqlPropertyValueRepository sql = new SqlPropertyValueRepository();
                await sql.CreateAsync(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        }

        #endregion

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Category = parameters.GetValue<Category>("category");
            Title = "Свойства группы " + Category.Title;
        }
    }
}

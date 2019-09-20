using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using ModelModul.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace PropertyModul.ViewModels
{
    public class ShowPropertyViewModel : EntityViewModelBase<PropertyName, PropertyNameViewModel, SqlPropertyNameRepository>, IEntitiesViewModelBase<PropertyValue, SqlPropertyValueRepository>
    {
        #region Properties

        public IDialogService DialogService { get; set; }

        private CancellationTokenSource _cancelTokenSource;

        public ObservableCollection<PropertyValue> EntitiesList
        {
            get => Entity.PropertyValuesCollection as ObservableCollection<PropertyValue>;
            set
            {
                Entity.PropertyValuesCollection = value;
                RaisePropertyChanged("EntitiesList");
                if (Entity.PropertyValuesCollection != null)
                    ((ObservableCollection<PropertyValue>) Entity.PropertyValuesCollection).CollectionChanged +=
                        PropertyValuesCollectionChanged;
            }
        }

        #endregion

        #region DelegateCommand

        public DelegateCommand<PropertyValue> AddEntityCommand { get; }
        public DelegateCommand<PropertyValue> SelectedEntityCommand { get; }
        public DelegateCommand<PropertyValue> DeleteEntityCommand { get; }

        #endregion

        public ShowPropertyViewModel(IDialogService dialogService) : base("Параметр добавлен", "Параметр изменен")
        {
            DialogService = dialogService;
            AddEntityCommand = new DelegateCommand<PropertyValue>(AddEntity);
            SelectedEntityCommand = new DelegateCommand<PropertyValue>(SelectedEntity);
            DeleteEntityCommand = new DelegateCommand<PropertyValue>(DeleteEntity);
        }

        public override void PropertiesTransfer(PropertyName fromEntity, PropertyName toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
            toEntity.IdCategory = fromEntity.IdCategory;
            toEntity.Category = fromEntity.Category;
        }

        private async void LoadAsync()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            try
            {
                IRepository<PropertyValue> propertyValueRepository = new SqlPropertyValueRepository();
                EntitiesList = new ObservableCollection<PropertyValue>(
                    await propertyValueRepository.GetListAsync(_cancelTokenSource.Token,
                        PropertyValueSpecification.GetPropertyValuesByIdPropertyName(Entity.Id)));
                if (EntitiesList != null)
                {
                    EntitiesList.CollectionChanged += PropertyValuesCollectionChanged;
                    foreach (var propertyValue in EntitiesList)
                    {
                        propertyValue.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (_cancelTokenSource == newCts)
                _cancelTokenSource = null;
        }

        private void PropertyValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs ea)
        {
            switch (ea.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (PropertyValue item in ea.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                    }

                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (PropertyValue item in ea.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                    }

                    break;
            }

            RaisePropertyChanged("EntitiesList");
        }

        private void AddEntity(PropertyValue obj)
        {
            DialogService.ShowDialog("ShowPropertyValue", new DialogParameters { { "propertyName", Entity.Id } }, CallbackProperty);
        }

        private void CallbackProperty(IDialogResult dialogResult)
        {
            PropertyValue temp = dialogResult.Parameters.GetValue<PropertyValue>("entity");
            if (temp == null) return;
            EntitiesList.Add(temp);
        }

        private void SelectedEntity(PropertyValue obj)
        {
            DialogService.Show("ShowPropertyValue", new DialogParameters { { "entity", obj } }, null);
        }

        private async void DeleteEntity(PropertyValue obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить значение?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<PropertyValue> propertyValueRepository = new SqlPropertyValueRepository();
                await propertyValueRepository.DeleteAsync(obj);
                EntitiesList.Remove(obj);
                MessageBox.Show("Значение удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Backup = parameters.GetValue<PropertyName>("entity");
                Entity = new PropertyNameViewModel();
                if (Backup == null)
                {
                    Title = "Добавить";
                    Entity.Category = parameters.GetValue<Category>("category");
                    Entity.IdCategory = Entity.Category?.Id;
                    IsAdd = true;
                }
                else
                {
                    Title = "Изменить";
                    IsAdd = false;
                    PropertiesTransfer(Backup, Entity);
                    LoadAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
            }
        }
    }
}

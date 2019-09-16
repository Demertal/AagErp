using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class EntitiesViewModelBase<TEntity, TRepository> : ViewModelBase, IEntitiesViewModelBase<TEntity, TRepository>
        where TEntity : ModelBase
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        private ObservableCollection<TEntity> _entitiesList = new ObservableCollection<TEntity>();
        public ObservableCollection<TEntity> EntitiesList
        {
            get => _entitiesList;
            set
            {
                SetProperty(ref _entitiesList, value);
                if (_entitiesList != null)
                    _entitiesList.CollectionChanged += CollectionChanged;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (TEntity item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= (o, ee) => RaisePropertyChanged("EntitiesList");
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (TEntity item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += (o, ee) => RaisePropertyChanged("EntitiesList");
                    }

                    break;
            }

            RaisePropertyChanged("EntitiesList");
        }

        private readonly string _dialogName, _askDelete, _successDelete;

        public DelegateCommand AddEntityCommand { get; }
        public DelegateCommand<TEntity> SelectedEntityCommand { get; }
        public DelegateCommand<TEntity> DeleteEntityCommand { get; }

        #endregion

        protected EntitiesViewModelBase(IDialogService dialogService, string dialogName, string askDelete, string successDelete) : base(dialogService)
        {
            _dialogName = dialogName;
            _askDelete = askDelete;
            _successDelete = successDelete;
            AddEntityCommand = new DelegateCommand(AddEntity);
            SelectedEntityCommand = new DelegateCommand<TEntity>(SelectedEntity);
            DeleteEntityCommand = new DelegateCommand<TEntity>(DeleteEntity);
            
        }

        private void AddEntity()
        {
            DialogService.ShowDialog(_dialogName, new DialogParameters(), CallbackProperty);
        }

        private void CallbackProperty(IDialogResult dialogResult)
        {
            TEntity temp = dialogResult.Parameters.GetValue<TEntity>("entity");
            if (temp == null) return;
            EntitiesList.Add(temp);
        }

        private void SelectedEntity(TEntity obj)
        {
            DialogService.Show(_dialogName, new DialogParameters { { "entity", obj } }, null);
        }

        private async void DeleteEntity(TEntity obj)
        {
            if (obj == null) return;
            if (MessageBox.Show(_askDelete, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<TEntity> repository = new TRepository();
                await repository.DeleteAsync(obj);
                EntitiesList.Remove(obj);
                MessageBox.Show(_successDelete, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async void LoadAsync()
        {
            try
            {
                IRepository<TEntity> repository = new TRepository();
                EntitiesList = new ObservableCollection<TEntity>(await repository.GetListAsync());
                foreach (var entity in EntitiesList)
                {
                    entity.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadAsync();
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

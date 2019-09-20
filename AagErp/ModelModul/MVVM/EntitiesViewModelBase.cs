using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class EntitiesViewModelBase<TEntity, TRepository> : ViewModelBase, IEntitiesViewModelBase<TEntity, TRepository>
        where TEntity : ModelBase<TEntity>
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        protected readonly string DialogName, AskDelete, SuccessDelete;

        protected CancellationTokenSource CancelTokenSource;

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

        #endregion

        #region Command

        public DelegateCommand<TEntity> AddEntityCommand { get; }
        public DelegateCommand<TEntity> SelectedEntityCommand { get; }
        public DelegateCommand<TEntity> DeleteEntityCommand { get; }

        #endregion

        protected EntitiesViewModelBase(IDialogService dialogService, string dialogName, string askDelete, string successDelete) : base(dialogService)
        {
            DialogName = dialogName;
            AskDelete = askDelete;
            SuccessDelete = successDelete;
            AddEntityCommand = new DelegateCommand<TEntity>(AddEntity);
            SelectedEntityCommand = new DelegateCommand<TEntity>(SelectedEntity);
            DeleteEntityCommand = new DelegateCommand<TEntity>(DeleteEntity);
            
        }

        protected virtual void AddEntity(TEntity obj)
        {
            DialogService.ShowDialog(DialogName, new DialogParameters(), CallbackEntity);
        }

        protected void CallbackEntity(IDialogResult dialogResult)
        {
            TEntity temp = dialogResult.Parameters.GetValue<TEntity>("entity");
            if (temp == null) return;
            AfterAddEntity(temp);
        }

        protected virtual void AfterAddEntity(TEntity obj)
        {
            EntitiesList.Add(obj);
        }

        private void SelectedEntity(TEntity obj)
        {
            DialogService.Show(DialogName, new DialogParameters { { "entity", obj } }, null);
        }

        protected virtual async void DeleteEntity(TEntity obj)
        {
            if (obj == null) return;
            if (MessageBox.Show(AskDelete, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<TEntity> repository = new TRepository();
                await repository.DeleteAsync(obj);
                EntitiesList.Remove(obj);
                MessageBox.Show(SuccessDelete, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        protected virtual async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                IRepository<TEntity> repository = new TRepository();
                EntitiesList = new ObservableCollection<TEntity>(await repository.GetListAsync(CancelTokenSource.Token));
                if(EntitiesList != null)
                    foreach (var entity in EntitiesList)
                    {
                        entity.PropertyChanged += (o, e) => RaisePropertyChanged("EntitiesList");
                    }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
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
            CancelTokenSource?.Cancel();
            CancelTokenSource = null;
        }

        #endregion
    }
}

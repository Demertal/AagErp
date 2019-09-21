using System;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class EntityViewModelBase<TEntity, TEntityViewModel, TRepository> : DialogViewModelBase, IEntityViewModelBase<TEntity, TEntityViewModel, TRepository>
        where TEntity : ModelBase<TEntity>
        where TEntityViewModel : TEntity, new()
        where TRepository : IRepository<TEntity>, new()

    {
        #region Properties

        private readonly string _messageAdd;
        private readonly string _messageUpdate;
        
        protected TEntity Backup;
        protected CancellationTokenSource CancelTokenSource;

        private TEntityViewModel _entity = new TEntityViewModel();
        public TEntityViewModel Entity
        {
            get => _entity;
            set
            {
                SetProperty(ref _entity, value);
                if (Entity != null)
                    Entity.PropertyChanged += (o, e) => { RaisePropertyChanged(); };
            }
        }

        private bool _isAdd;
        public bool IsAdd
        {
            get => _isAdd;
            set => SetProperty(ref _isAdd, value);
        }


        #endregion

        #region Command

        public DelegateCommand AcceptCommand { get; }

        #endregion

        protected EntityViewModelBase(string messageAdd, string messageUpdate)
        {
            _messageAdd = messageAdd;
            _messageUpdate = messageUpdate;
            AcceptCommand = new DelegateCommand(AcceptAsync).ObservesCanExecute(() => Entity.IsValid);
        }

        private async void AcceptAsync()
        {
            try
            {
                string message;
                TEntity temp = (TEntity)Entity.Clone();
                IRepository<TEntity> repository = new TRepository();
                if (IsAdd)
                {
                    await repository.CreateAsync(temp);
                    message = _messageAdd;
                }
                else
                {
                    await repository.UpdateAsync(temp);
                    message = _messageUpdate;
                }
                MessageBox.Show(message, "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                if (Backup != null)
                {
                    PropertiesTransfer(temp, Backup);
                }
                RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "entity", temp } }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public abstract void PropertiesTransfer(TEntity fromEntity, TEntity toEntity);

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Backup = parameters.GetValue<TEntity>("entity");
                Entity = new TEntityViewModel();
                if (Backup == null)
                {
                    Title = "Добавить";
                    IsAdd = true;
                }
                else
                {
                    Title = "Изменить";
                    IsAdd = false;
                    PropertiesTransfer(Backup, Entity);
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

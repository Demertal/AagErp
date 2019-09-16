using System.Collections.ObjectModel;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public interface IEntitiesViewModelBase<TEntity, TRepository>
        where TEntity : ModelBase
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        ObservableCollection<TEntity> EntitiesList { get; set; }

        DelegateCommand AddEntityCommand { get; }
        DelegateCommand<TEntity> SelectedEntityCommand { get; }
        DelegateCommand<TEntity> DeleteEntityCommand { get; }

        #endregion
    }
}

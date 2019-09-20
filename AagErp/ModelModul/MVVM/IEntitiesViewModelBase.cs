using System.Collections.ObjectModel;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;

namespace ModelModul.MVVM
{
    public interface IEntitiesViewModelBase<TEntity, TRepository>
        where TEntity : ModelBase<TEntity>
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        ObservableCollection<TEntity> EntitiesList { get; set; }

        #endregion

        #region Command

        DelegateCommand<TEntity> AddEntityCommand { get; }
        DelegateCommand<TEntity> SelectedEntityCommand { get; }
        DelegateCommand<TEntity> DeleteEntityCommand { get; }

        #endregion
    }
}

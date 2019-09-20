using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;

namespace ModelModul.MVVM
{
    interface IEntityViewModelBase<in TEntity, TEntityViewModel, TRepository>
        where TEntity : ModelBase<TEntity>
        where TEntityViewModel : TEntity, new()
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        TEntityViewModel Entity { get; set; }

        bool IsAdd { get; set; }

        #endregion

        #region Command

        DelegateCommand AcceptCommand { get; }

        #endregion

        void PropertiesTransfer(TEntity fromEntity, TEntity toEntity);
    }
}

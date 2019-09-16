using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class ViewModelBase: BindableBase, INavigationAware
    {
        protected readonly IDialogService DialogService;

        protected ViewModelBase(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        #region INavigationAware

        public abstract void OnNavigatedTo(NavigationContext navigationContext);

        public abstract bool IsNavigationTarget(NavigationContext navigationContext);

        public abstract void OnNavigatedFrom(NavigationContext navigationContext);

        #endregion
    }
}

using ModelModul;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class InfoReport<T>: ViewModelBase
    {
        private T _report;
        public T Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
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

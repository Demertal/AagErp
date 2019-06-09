using Prism.Mvvm;

namespace ReportModul.ViewModels
{
    class InfoReport<T>: BindableBase
    {
        private T _report;
        public T Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }
    }
}

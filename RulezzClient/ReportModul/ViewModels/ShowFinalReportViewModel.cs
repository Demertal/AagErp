using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul.Product;
using ModelModul.Report;
using Prism.Mvvm;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowFinalReportViewModel: BindableBase, INavigationAware
    {
        #region Property

        private ObservableCollection<FinalReportProductViewModel> _reportList = new ObservableCollection<FinalReportProductViewModel>();
        public ObservableCollection<FinalReportProductViewModel> ReportList
        {
            get => _reportList;
            set => SetProperty(ref _reportList, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        #endregion

        public ShowFinalReportViewModel()
        {
            EndDate = DateTime.Today;
            StartDate = DateTime.Today;
            LoadAsync();
        }

        private async void LoadAsync()
        {
            try
            {
                DbSetReports dbSet = new DbSetReports();
                ReportList = await dbSet.GetFinalReport(StartDate, EndDate);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}

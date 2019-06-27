using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.Report;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowFinalReportViewModel: ViewModelBase
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
            set
            {
                SetProperty(ref _startDate, value);
                Load();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                Load();
            }
        }

        public string FinalSum
        {
            get { return "Итого прибыль: " + ReportList.Sum(obj => obj.FinalSum).ToString("C", new CultureInfo("UA-ua")); }
        }

        #endregion

        public ShowFinalReportViewModel()
        {
            EndDate = DateTime.Today;
            StartDate = DateTime.Today;
            Load();
        }

        private void Load()
        {
            try
            {
                DbSetReports dbSet = new DbSetReports();
                ReportList = dbSet.GetFinalReport(StartDate, EndDate);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            RaisePropertyChanged("FinalSum");
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

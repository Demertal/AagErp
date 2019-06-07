using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul.Product;
using ModelModul.Report;
using Prism.Mvvm;

namespace ReportModul.ViewModels
{
    class ShowFinalReportViewModel: BindableBase
    {
        #region Property

        private ObservableCollection<FinalReportProductViewModel> _reportList = new ObservableCollection<FinalReportProductViewModel>();

        public ObservableCollection<FinalReportProductViewModel> ReportList
        {
            get => _reportList;
            set => SetProperty(ref _reportList, value);
        }

        #endregion

        public ShowFinalReportViewModel()
        {
            LoadAsync();
        }

        private void LoadAsync()
        {
            try
            {
                DbSetReports dbSet = new DbSetReports();
                //ReportList = await dbSet.GetFinalReport();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

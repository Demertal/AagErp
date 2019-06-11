using System.ComponentModel;
using System.Windows.Controls;
using ModelModul;
using Prism.Common;
using Prism.Regions;
using ReportModul.ViewModels;

namespace ReportModul.Views
{
    /// <summary>
    /// Логика взаимодействия для SalesReportInfo.xaml
    /// </summary>
    public partial class SalesReportInfo : UserControl
    {
        public SalesReportInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += RevaluationReportInfo_PropertyChanged;
        }

        private void RevaluationReportInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var salesReport = (SalesReports)context.Value;
            (DataContext as SalesReportInfoViewModel).Report = salesReport ?? new SalesReports();
        }
    }
}

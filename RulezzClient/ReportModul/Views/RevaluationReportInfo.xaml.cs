using System.ComponentModel;
using System.Windows.Controls;
using ModelModul.Models;
using Prism.Common;
using Prism.Regions;
using ReportModul.ViewModels;

namespace ReportModul.Views
{
    /// <summary>
    /// Логика взаимодействия для RevaluationReportInfo.xaml
    /// </summary>
    public partial class RevaluationReportInfo : UserControl
    {
        public RevaluationReportInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += RevaluationReportInfo_PropertyChanged;
        }

        private void RevaluationReportInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var revaluationProductsReport = (RevaluationProducts)context.Value;
            (DataContext as RevaluationReportInfoViewModel).Report = revaluationProductsReport ?? new RevaluationProducts();
        }
    }
}

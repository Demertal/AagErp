using System.Windows.Controls;
using ModelModul;
using Prism.Common;
using Prism.Regions;
using ReportModul.ViewModels;

namespace ReportModul.Views
{
    /// <summary>
    /// Логика взаимодействия для PurchaseReportInfo.xaml
    /// </summary>
    public partial class PurchaseReportInfo : UserControl
    {
        public PurchaseReportInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += PurchaseReportInfo_PropertyChanged;
        }

        private void PurchaseReportInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var purchaseReports = (PurchaseReports)context.Value;
            (DataContext as PurchaseReportInfoViewModel).PurchaseReport = purchaseReports ?? new PurchaseReports();
        }
    }
}

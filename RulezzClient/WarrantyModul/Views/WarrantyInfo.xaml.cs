using System.ComponentModel;
using System.Windows.Controls;
using ModelModul.Warranty;
using Prism.Common;
using Prism.Regions;
using WarrantyModul.ViewModels;

namespace WarrantyModul.Views
{
    /// <summary>
    /// Логика взаимодействия для WarrantyInfo.xaml
    /// </summary>
    public partial class WarrantyInfo : UserControl
    {
        public WarrantyInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += WarrantyInfo_PropertyChanged;
        }

        private void WarrantyInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var selectedWarranty = (WarrantyViewModel)context.Value;
            (DataContext as WarrantyInfoViewModel).SelectedWarranty = selectedWarranty ?? new WarrantyViewModel();
        }
    }
}

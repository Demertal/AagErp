using System.Windows.Controls;
using ModelModul.Product;
using Prism.Common;
using Prism.Regions;
using ProductModul.ViewModels;

namespace ProductModul.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductInfo.xaml
    /// </summary>
    public partial class ProductInfo : UserControl
    {
        public ProductInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += ProductInfo_PropertyChanged;
        }

        private void ProductInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var selectedProduct = (ProductViewModel)context.Value;
            (DataContext as ProductInfoViewModel).SelectedProduct = selectedProduct ?? new ProductViewModel();
        }
    }
}

using System.ComponentModel;
using System.Windows.Controls;
using CounterpartyModul.ViewModels;
using ModelModul;
using Prism.Common;
using Prism.Regions;

namespace CounterpartyModul.Views
{
    /// <summary>
    /// Логика взаимодействия для CounterpartyInfo.xaml
    /// </summary>
    public partial class CounterpartyInfo : UserControl
    {
        public CounterpartyInfo()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += CounterpartyInfo_PropertyChanged;
        }

        private void CounterpartyInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            //var selectedCounterparty = (CounterpartyViewModel)context.Value;
            //(DataContext as CounterpartyInfoViewModel).SelectedCounterparty = selectedCounterparty ?? new CounterpartyViewModel();
        }
    }
}

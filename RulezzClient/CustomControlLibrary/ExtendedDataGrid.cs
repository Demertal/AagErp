using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;

namespace CustomControlLibrary
{
    public class ExtendedDataGrid: DataGrid
    {
        public ExtendedDataGrid()
        {
            CmHeaderCommand = new DelegateCommand<int?>(idx =>
            {
                if(idx == null) return;
                Columns[idx.Value].Visibility = Columns[idx.Value].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            });
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            var obj = (DependencyObject)e.OriginalSource;

            while (obj != null && !(obj is DataGridColumnHeader))
                obj = VisualTreeHelper.GetParent(obj);

            if (obj == null)
                return;

            ContextMenu cmHeader = new ContextMenu();
            foreach (var col in Columns)
            {
                cmHeader.Items.Add(new MenuItem { Header = col.Header, Command = CmHeaderCommand, CommandParameter = col.DisplayIndex, IsChecked = col.Visibility == Visibility.Visible });
            }
            cmHeader.IsOpen = true;

            base.OnMouseRightButtonDown(e);
        }

        public DelegateCommand<int?> CmHeaderCommand { get; }
    }
}

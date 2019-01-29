using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RulezzClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ShowGroup.xaml
    /// </summary>
    public partial class ShowGroup : UserControl
    {
        public ShowGroup()
        {
            InitializeComponent();
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}

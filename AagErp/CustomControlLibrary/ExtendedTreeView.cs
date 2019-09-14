using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLibrary
{
    public class ExtendedTreeView : TreeView
    {
        #region Properies

        public TreeViewItem SelectedItemTv;

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));

        #endregion

        public ExtendedTreeView() : base()
        {
            SelectedItemChanged += ___ICH;
            PreviewMouseRightButtonDown += ___MRBD;
            PreviewMouseLeftButtonDown += ___MLBD;
        }

        #region EventHandlers

        private void ___ICH(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetValue(SelectedItemProperty, base.SelectedItem);
        }

        private void ___MRBD(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            treeViewItem?.Focus();
            if (treeViewItem != null)
            {
                SelectedItemTv = treeViewItem;
                return;
            }

            if (SelectedItemTv == null)
            {
                ((ExtendedTreeView)sender).Focus();
                return;
            }
            SelectedItemTv.IsSelected = false;
            SelectedItemTv = null;
            SetValue(SelectedItemProperty, null);
        }

        private void ___MLBD(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (treeViewItem != null)
            {
                SelectedItemTv = treeViewItem;
                return;
            }

            if (SelectedItemTv == null) return;
            SelectedItemTv.IsSelected = false;
            SelectedItemTv = null;
            SetValue(SelectedItemProperty, null);
        }

        #endregion

        private static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLibrary
{
    public class ExtendedTreeView : TreeView
    {
        #region Properies

        private TreeViewItem _selectedItemTv;

        public new object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public new static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));

        #endregion

        public ExtendedTreeView()
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
                _selectedItemTv = treeViewItem;
                return;
            }

            if (_selectedItemTv == null) return;
            _selectedItemTv.IsSelected = false;
            _selectedItemTv = null;
            SetValue(SelectedItemProperty, null);
        }

        private void ___MLBD(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (treeViewItem != null)
            {
                _selectedItemTv = treeViewItem;
                return;
            }

            if (_selectedItemTv == null) return;
            _selectedItemTv.IsSelected = false;
            _selectedItemTv = null;
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

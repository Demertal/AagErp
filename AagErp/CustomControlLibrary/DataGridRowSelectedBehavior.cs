using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControlLibrary
{
    public class DataGridRowSelectedBehavior
    {
        public static ICommand GetCommand(DataGridRow row)
        {
            return (ICommand)row.GetValue(CommandProperty);
        }

        public static void SetCommand(DataGridRow row, ICommand value)
        {
            row.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(DataGridRowSelectedBehavior),
                new UIPropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridRow row) row.MouseDoubleClick += RowOnMouseDoubleClick;
        }

        private static void RowOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DataGridRow row = sender as DataGridRow;
            ICommand command = GetCommand(row);
            if (row != null) command?.Execute(row.DataContext);
        }
    }
}

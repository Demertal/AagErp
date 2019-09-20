using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControlLibrary
{
    public class MouseDoubleClickCommandBehavior
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
            typeof(ICommand), typeof(MouseDoubleClickCommandBehavior), new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MouseDoubleClickCommandBehavior), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }
        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Control control)) return;
            if (e.NewValue != null && e.OldValue == null)
            {
                control.MouseDoubleClick += OnMouseDoubleClick;
            }
            else if (e.NewValue == null && e.OldValue != null)
            {
                control.MouseDoubleClick -= OnMouseDoubleClick;
            }
        }

        private static void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Control control)) return;
            ICommand command = (ICommand)control.GetValue(CommandProperty);
            if(CommandParameterProperty != null)
            {
                object commandParameter = control.GetValue(CommandParameterProperty);
                command.Execute(commandParameter);
            }
            else
            {
                command.Execute(control.DataContext);
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace CustomControlLibrary
{
    public class ChangeableTextBlock: TextBox
    {
        public static string _oldText;
        public static DependencyProperty IsChangeProperty;

        static ChangeableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangeableTextBlock), new FrameworkPropertyMetadata(typeof(ChangeableTextBlock)));
            IsChangeProperty = DependencyProperty.Register("IsChange", typeof(bool), typeof(ChangeableTextBlock));
        }

        public bool IsChange
        {
            get => (bool)GetValue(IsChangeProperty);
            set => SetValue(IsChangeProperty, value);
        }
    }
}

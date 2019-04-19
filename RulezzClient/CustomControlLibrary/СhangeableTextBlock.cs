using System.Windows;
using System.Windows.Controls;

namespace CustomControlLibrary
{
    public class СhangeableTextBlock: TextBox
    {
        public static string _oldText;
        public static DependencyProperty IsСhangeProperty;

        static СhangeableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(СhangeableTextBlock), new FrameworkPropertyMetadata(typeof(СhangeableTextBlock)));
            IsСhangeProperty = DependencyProperty.Register("IsСhange", typeof(bool), typeof(СhangeableTextBlock));
        }

        public bool IsСhange
        {
            get => (bool)GetValue(IsСhangeProperty);
            set => SetValue(IsСhangeProperty, value);
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace CustomControlLibrary.CustomControl
{
    public class WatermarkTextBox : TextBox
    {
        public static DependencyProperty WatermarkTextProperty;

        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
            WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkTextBox));
        }

        public string WatermarkText
        {
            get => (string) GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }
    }
}

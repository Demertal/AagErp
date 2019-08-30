using System.Windows;
using System.Windows.Controls;

namespace ModelModul
{
    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty IsWeightGoodsProperty = DependencyProperty.Register("IsWeightGoods", typeof(bool), typeof(Wrapper), new FrameworkPropertyMetadata(false));

        public bool IsWeightGoods
        {
            get => (bool)GetValue(IsWeightGoodsProperty);
            set => SetValue(IsWeightGoodsProperty, value);
        }
    }

    public class CountValidationRule : ValidationRule
    {
        public Wrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            string count = value as string;
            if (!decimal.TryParse(count, out var result)) return new ValidationResult(false, "Это не число");
            if (!Wrapper.IsWeightGoods && count.IndexOfAny(new[] { '.', ',' }) != -1)
                return new ValidationResult(false, "Кол-во не может быть дробным");
            return ValidationResult.ValidResult;
        }
    }
}

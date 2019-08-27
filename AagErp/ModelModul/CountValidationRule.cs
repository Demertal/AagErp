using System.Windows.Controls;

namespace ModelModul
{
    public class CountValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            string count = value as string;
            return !double.TryParse(count, out var result) ? new ValidationResult(false, "Это не число") : ValidationResult.ValidResult;
        }
    }
}

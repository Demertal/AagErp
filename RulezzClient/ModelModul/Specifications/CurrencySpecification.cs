using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class CurrencySpecification
    {
        public static ExpressionSpecification<Currency> GetDefaultCurrency()
        {
            return new ExpressionSpecification<Currency>(obj => obj.IsDefault);
        }
    }
}

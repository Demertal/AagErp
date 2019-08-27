using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class MovmentGoodTypeSpecification
    {
        public static ExpressionSpecification<MovmentGoodType> GetMovmentGoodTypeByCode(string code)
        {
            return new ExpressionSpecification<MovmentGoodType>(obj => obj.Code == code);
        }
    }
}

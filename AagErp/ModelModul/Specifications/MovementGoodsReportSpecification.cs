using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class MovementGoodsReportSpecification
    {
        public static ExpressionSpecification<MovementGoods> GetMovementGoodsReportsByType(string code)
        {
            return new ExpressionSpecification<MovementGoods>(obj => obj.MovmentGoodType.Code == code);
        }
    }
}

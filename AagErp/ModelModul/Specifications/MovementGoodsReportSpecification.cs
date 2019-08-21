using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class MovementGoodsReportSpecification
    {
        public static ExpressionSpecification<MovementGoods> GetMovementGoodsReportsByType(int type)
        {
            return new ExpressionSpecification<MovementGoods>(obj => obj.IdType == type);
        }
    }
}

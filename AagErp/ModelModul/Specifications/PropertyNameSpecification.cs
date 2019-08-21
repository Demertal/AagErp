using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class PropertyNameSpecification
    {
        public static ExpressionSpecification<PropertyName> GetPropertyNamesByIdGroup(int idGroup)
        {
            return new ExpressionSpecification<PropertyName>(obj => obj.IdCategory == idGroup);
        }
    }
}

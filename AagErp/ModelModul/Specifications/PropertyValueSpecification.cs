using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class PropertyValueSpecification
    {
        public static ExpressionSpecification<PropertyValue> GetPropertyValuesByIdPropertyName(int idPropertyName)
        {
            return new ExpressionSpecification<PropertyValue>(obj => obj.IdPropertyName == idPropertyName);
        }
    }
}

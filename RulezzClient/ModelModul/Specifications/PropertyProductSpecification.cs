using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class PropertyProductSpecification
    {
        public static ExpressionSpecification<PropertyProduct> GetPropertyProductsByIdProduct(int idProduct)
        {
            return new ExpressionSpecification<PropertyProduct>(obj => obj.IdProduct == idProduct);
        }
    }
}

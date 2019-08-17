using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class CategorySpecification
    {
        public static ExpressionSpecification<Category> GetCategoriesByIdParent(int? idParent = null)
        {
            return new ExpressionSpecification<Category>(obj => obj.IdParent == idParent);
        }
    }
}

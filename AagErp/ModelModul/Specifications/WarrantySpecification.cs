using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class WarrantySpecification
    {
        public static ExpressionSpecification<Warranty> GetWarrantiesByIdSerialNumber(int idSerialNumber)
        {
            return new ExpressionSpecification<Warranty>(obj => obj.IdSerialNumber == idSerialNumber);
        }
    }
}

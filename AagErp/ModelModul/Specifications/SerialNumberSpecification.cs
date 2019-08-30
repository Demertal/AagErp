using Microsoft.EntityFrameworkCore;
using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class SerialNumberSpecification
    {
        public static ExpressionSpecification<SerialNumber> GetSerialNumbersByContainsValue(string value)
        {
            if (value != "") value = "%" + value + "%";
            return new ExpressionSpecification<SerialNumber>(obj => EF.Functions.Like(obj.Value, value));
        }

        public static ExpressionSpecification<SerialNumber> GetSerialNumbersValue(string value)
        {
            return new ExpressionSpecification<SerialNumber>(obj => obj.Value == value);
        }

        public static ExpressionSpecification<SerialNumber> GetSerialNumbersByIdProduct(long idProduct)
        {
            return new ExpressionSpecification<SerialNumber>(obj => obj.IdProduct == idProduct);
        }

        public static ExpressionSpecification<SerialNumber> GetSerialNumbersByIdProductAndValue(long idProduct, string value, int idStore)
        {
            return new ExpressionSpecification<SerialNumber>(GetSerialNumbersByIdProduct(idProduct).And(GetSerialNumbersByContainsValue(value)).IsSatisfiedBy());
        }
    }
}

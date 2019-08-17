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

        public static ExpressionSpecification<SerialNumber> GetSerialNumbersByIdProduct(int idProduct)
        {
            return new ExpressionSpecification<SerialNumber>(obj => obj.IdProduct == idProduct);
        }

        public static ExpressionSpecification<SerialNumber> GetFreeSerialNumbers(string value, int idProduct)
        {
            return null;
            //return new ExpressionSpecification<SerialNumber>(GetSerialNumbersByContainsValue(value)
            //    .And(GetSerialNumbersByIdProduct(idProduct)
            //        .And(new ExpressionSpecification<SerialNumber>(obj => obj.IdSaleReport == null))).IsSatisfiedBy());
        }
    }
}

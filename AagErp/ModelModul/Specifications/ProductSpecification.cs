using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class ProductSpecification
    {
        public static ExpressionSpecification<Product> GetProductsByBarcode(string barcode)
        {
            return new ExpressionSpecification<Product>(obj => obj.Barcode == barcode);
        }

        public static ExpressionSpecification<Product> GetProductsByLikeBarcode(string barcode)
        {
            barcode = "%" + barcode + "%";
            return new ExpressionSpecification<Product>(obj => EF.Functions.Like(obj.Barcode, barcode));
        }

        public static ExpressionSpecification<Product> GetProductsByLikeTitle(string title)
        {
            title = "%" + title + "%";
            return new ExpressionSpecification<Product>(obj => EF.Functions.Like(obj.Title, title));
        }

        public static ExpressionSpecification<Product> GetProductsByContainsVendorCode(string vendorCode)
        {
            vendorCode = "%" + vendorCode + "%";
            return new ExpressionSpecification<Product>(obj => EF.Functions.Like(obj.VendorCode, vendorCode));
        }

        public static ExpressionSpecification<Product> GetProductsByFindString(string findString)
        {
            return new ExpressionSpecification<Product>(GetProductsByLikeBarcode(findString)
                .Or(GetProductsByContainsVendorCode(findString).Or(GetProductsByLikeTitle(findString)))
                .IsSatisfiedBy());
        }

        public static ExpressionSpecification<Product> GetProductsByIdGroup(int idGroup)
        {
            return new ExpressionSpecification<Product>(obj => obj.IdCategory == idGroup);
        }

        public static ExpressionSpecification<Product> GetProductsByIdGroupOrFindString(int? idGroup, string findString)
        {
            return idGroup == null || idGroup == 0
                ? new ExpressionSpecification<Product>(GetProductsByFindString(findString).IsSatisfiedBy())
                : new ExpressionSpecification<Product>(GetProductsByIdGroup(idGroup.Value)
                    .And(GetProductsByFindString(findString)).IsSatisfiedBy());
        }

        public static ExpressionSpecification<Product> GetProductsByIdGroupOrFindStringOrProperty(int? idGroup, string findString, ICollection<PropertyProduct> properties)
        {
            if (properties == null || properties.All(p => p.IdPropertyValue == null || p.IdPropertyValue == 0))
                return GetProductsByIdGroupOrFindString(idGroup, findString);
            var temp = new ExpressionSpecification<Product>(GetProductsByIdGroupOrFindString(idGroup, findString).IsSatisfiedBy());
            foreach (var property in properties.Where(p => p.IdPropertyValue != null))
            {
                temp = new ExpressionSpecification<Product>(temp
                    .And(new ExpressionSpecification<Product>(pp =>
                        pp.PropertyProductsCollection.Any(p => p.IdPropertyValue == property.IdPropertyValue)))
                    .IsSatisfiedBy());
            }

            return temp;
        }

        public static ExpressionSpecification<Product> GetProductsById([Annotations.NotNull]params long[] idCollection)
        {
            var temp = new ExpressionSpecification<Product>(p => p.Id == idCollection[0]);
            for (int i = 1; i < idCollection.Length; i++)
            {
                temp = new ExpressionSpecification<Product>(temp
                    .Or(new ExpressionSpecification<Product>(p => p.Id == idCollection[i]))
                    .IsSatisfiedBy());
            }

            return temp;
        }
    }
}

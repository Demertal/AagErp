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
            if (barcode != "") barcode = "%" + barcode + "%";
            return new ExpressionSpecification<Product>(obj => EF.Functions.Like(obj.Barcode, barcode));
        }

        public static ExpressionSpecification<Product> GetProductsByLikeTitle(string title)
        {
            if (title != "") title = "%" + title + "%";
            return new ExpressionSpecification<Product>(obj => EF.Functions.Like(obj.Title, title));
        }

        public static ExpressionSpecification<Product> GetProductsByContainsVendorCode(string vendorCode)
        {
            if (vendorCode != "") vendorCode = "%" + vendorCode + "%";
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
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(256);
            builder.Property(p => p.VendorCode).HasMaxLength(20);
            builder.Property(p => p.Barcode).HasMaxLength(13);
            builder.Property(p => p.Description).HasColumnType("nvarchar(max)");
            builder.Ignore(p => p.CountsProductCollection);
            builder.Ignore(p => p.IsValidate);
            builder.Ignore(p => p.EquivalentCostForExistingProductsCollection);
            builder.Ignore(p => p.Count);
            builder.Ignore(p => p.Price);
            builder.Ignore(p => p.Error);
            builder.HasMany(p => p.PropertyProductsCollection).WithOne(p => p.Product).HasForeignKey(p => p.IdProduct)
                .IsRequired();
            builder.HasMany(p => p.InvoiceInfosCollection).WithOne(i => i.Product).HasForeignKey(i => i.IdProduct).IsRequired();
            builder.HasMany(p => p.MovementGoodsInfosCollection).WithOne(m => m.Product).HasForeignKey(m => m.IdProduct)
                .IsRequired();
            builder.HasMany(p => p.SerialNumbersCollection).WithOne(s => s.Product).HasForeignKey(s => s.IdProduct).IsRequired();
        }
    }
}

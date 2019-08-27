using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            builder.Property(p => p.Price).HasColumnType("money").Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Property(p => p.Count).Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Ignore(p => p.CountsProduct);
            builder.Ignore(p => p.IsValidate);
            builder.Ignore(p => p.EquivalentCostForЕxistingProducts);
            builder.Ignore(p => p.Error);
            builder.HasMany(p => p.InvoiceInfos).WithOne(i => i.Product).HasForeignKey(i => i.IdProduct).IsRequired();
            builder.HasMany(p => p.MovementGoodsInfos).WithOne(m => m.Product).HasForeignKey(m => m.IdProduct)
                .IsRequired();
            builder.HasMany(p => p.SerialNumbers).WithOne(s => s.Product).HasForeignKey(s => s.IdProduct).IsRequired();
        }
    }
}

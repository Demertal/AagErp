using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PriceGroupConfiguration : IEntityTypeConfiguration<PriceGroup>
    {

        public void Configure(EntityTypeBuilder<PriceGroup> builder)
        {
            builder.ToTable("priceGroups");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Markup).IsRequired().HasColumnType("decimal(7,4)");
            builder.HasMany(p => p.ProductsCollection).WithOne(p => p.PriceGroup).HasForeignKey(p => p.IdPriceGroup).IsRequired();
        }
    }
}

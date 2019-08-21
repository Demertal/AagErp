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
            builder.HasMany(p => p.Products).WithOne(p => p.PriceGroup).HasForeignKey(p => p.IdPriceGroup);
            builder.Property(p => p.Markup).IsRequired().HasColumnType("decimal(5,2)");
        }
    }
}

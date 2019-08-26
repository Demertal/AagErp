using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PriceProductConfiguration : IEntityTypeConfiguration<PriceProduct>
    {
        public void Configure(EntityTypeBuilder<PriceProduct> builder)
        {
            builder.ToTable("priceProducts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Price).HasColumnType("money");
            builder.HasOne(p => p.Product).WithMany(p => p.PriceProducts).HasForeignKey(p => p.IdProduct).IsRequired();
        }
    }
}

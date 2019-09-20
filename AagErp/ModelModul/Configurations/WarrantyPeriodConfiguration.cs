using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class WarrantyPeriodConfiguration : IEntityTypeConfiguration<WarrantyPeriod>
    {
        public void Configure(EntityTypeBuilder<WarrantyPeriod> builder)
        {
            builder.ToTable("warrantyPeriods");
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Period).IsRequired().HasMaxLength(20);
            builder.Ignore(c => c.ValidationRules);
            builder.HasMany(w => w.ProductsCollection).WithOne(p => p.WarrantyPeriod).HasForeignKey(p => p.IdWarrantyPeriod).IsRequired();
        }
    }
}

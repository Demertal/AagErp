using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class UnitStorageConfiguration : IEntityTypeConfiguration<UnitStorage>
    {
        public void Configure(EntityTypeBuilder<UnitStorage> builder)
        {
            builder.ToTable("unitStorages");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title).IsRequired().HasMaxLength(20);
            builder.HasMany(u => u.Products).WithOne(p => p.UnitStorage).HasForeignKey(p => p.IdUnitStorage)
                .IsRequired();
        }
    }
}
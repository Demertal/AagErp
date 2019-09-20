using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("stores");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Title).IsRequired().HasMaxLength(50);
            builder.Ignore(c => c.ValidationRules);
            builder.HasMany(s => s.ArrivalMovementGoodsCollection).WithOne(m => m.ArrivalStore).HasForeignKey(m => m.IdArrivalStore);
            builder.HasMany(s => s.DisposalMovementGoodsCollection).WithOne(m => m.DisposalStore).HasForeignKey(m => m.IdDisposalStore);
        }
    }
}

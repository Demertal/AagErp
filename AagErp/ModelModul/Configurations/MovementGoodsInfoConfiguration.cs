using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class MovementGoodsInfoConfiguration : IEntityTypeConfiguration<MovementGoodsInfo>
    {
        public void Configure(EntityTypeBuilder<MovementGoodsInfo> builder)
        {
            builder.ToTable("movementGoodsInfos");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Price).HasColumnType("money");
            builder.Property(m => m.EquivalentCost).HasColumnType("money");
        }
    }
}

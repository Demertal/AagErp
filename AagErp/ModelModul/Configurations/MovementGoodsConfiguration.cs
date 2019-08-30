using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class MovementGoodsConfiguration : IEntityTypeConfiguration<MovementGoods>
    {
        public void Configure(EntityTypeBuilder<MovementGoods> builder)
        {
            builder.ToTable("movementGoods");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Rate).HasColumnType("money");
            builder.Property(m => m.EquivalentRate).HasColumnType("money");
            builder.Property(m => m.TextInfo).HasMaxLength(50);
            builder.Property(m => m.DateCreate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(m => m.DateClose).HasColumnType("datetime");
            builder.Ignore(m => m.Error);
            builder.HasMany(m => m.MoneyTransfersCollection).WithOne(m => m.MovementGoods).HasForeignKey(m => m.IdMovementGoods).IsRequired();
            builder.HasMany(m => m.MovementGoodsInfosCollection).WithOne(m => m.MovementGoods).HasForeignKey(m => m.IdReport).IsRequired();
            builder.HasMany(m => m.SerialNumberLogsCollection).WithOne(s => s.MovementGood).HasForeignKey(s => s.IdMovmentGood).IsRequired();
            builder.HasMany(m => m.MovementGoodsCollection).WithOne(m => m.MovementGood)
                .HasForeignKey(m => m.IdMovementGood);
        }
    }
}

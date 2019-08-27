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
            builder.HasMany(m => m.MoneyTransfers).WithOne(m => m.MovementGoods).HasForeignKey(m => m.IdMovementGoods).IsRequired();
            builder.HasMany(m => m.MovementGoodsInfos).WithOne(m => m.MovementGoods).HasForeignKey(m => m.IdReport).IsRequired();
            builder.HasMany(m => m.SerialNumberLogs).WithOne(s => s.MovementGood).HasForeignKey(s => s.IdMovmentGood).IsRequired();
        }
    }
}

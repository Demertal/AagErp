using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("currencies");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(10);
            builder.Property(c => c.Cost).HasColumnType("money");
            builder.HasMany(c => c.MovementGoods).WithOne(m => m.Currency).HasForeignKey(m => m.IdCurrency);
            builder.HasMany(c => c.MovementGoodsEquivalent).WithOne(m => m.EquivalentCurrency).HasForeignKey(m => m.IdEquivalentCurrency);
        }
    }
}

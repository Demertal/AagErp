using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class CounterpartyConfiguration : IEntityTypeConfiguration<Counterparty>
    {
        public void Configure(EntityTypeBuilder<Counterparty> builder)
        {
            builder.ToTable("counterparties");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(40);
            builder.Property(c => c.ContactPerson).HasMaxLength(40);
            builder.Property(c => c.ContactPhone).HasMaxLength(50);
            builder.Property(c => c.Props).HasMaxLength(40);
            builder.Property(c => c.Address).HasMaxLength(40);
            builder.Ignore(c => c.Error);
            builder.HasMany(c => c.MoneyTransfersCollection).WithOne(m => m.Counterparty).HasForeignKey(m => m.IdCounterparty)
                .IsRequired();
            builder.HasMany(c => c.MovementGoodsCollection).WithOne(m => m.Counterparty).HasForeignKey(m => m.IdCounterparty);
        }
    }
}

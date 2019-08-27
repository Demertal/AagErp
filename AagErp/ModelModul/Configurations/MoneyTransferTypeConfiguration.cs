using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class MoneyTransferTypeConfiguration : IEntityTypeConfiguration<MoneyTransferType>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferType> builder)
        {
            builder.ToTable("moneyTransferTypes");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Code).IsRequired().HasMaxLength(20);
            builder.Property(m => m.Description).IsRequired().HasMaxLength(50);
            builder.Ignore(m => m.Error);
            builder.HasMany(m => m.MoneyTransfers).WithOne(m => m.MoneyTransferType).HasForeignKey(m => m.IdType).IsRequired();
        }
    }
}

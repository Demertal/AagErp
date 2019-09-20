using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
    {
        public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
        {
            builder.ToTable("moneyTransfers");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.MoneyAmount).HasColumnType("money");
            builder.Ignore(c => c.ValidationRules);
        }
    }
}

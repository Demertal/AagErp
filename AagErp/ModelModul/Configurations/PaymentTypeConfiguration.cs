using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.ToTable("paymentTypes");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(50);
            builder.Ignore(p => p.Error);
            builder.HasMany(p => p.Counterparties).WithOne(c => c.PaymentType).HasForeignKey(c => c.IdPaymentType).IsRequired();
        }
    }
}

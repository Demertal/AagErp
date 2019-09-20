using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class InvoiceInfoConfiguration : IEntityTypeConfiguration<InvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<InvoiceInfo> builder)
        {
            builder.ToTable("invoiceInfos");
            builder.HasKey(i => i.Id);
            builder.Ignore(c => c.ValidationRules);
        }
    }
}

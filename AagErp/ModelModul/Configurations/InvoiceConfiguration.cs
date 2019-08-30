using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("invoices");
            builder.Ignore(i => i.Error);
            builder.HasKey(c => c.Id);
            builder.HasMany(i => i.InvoiceInfosCollection).WithOne(i => i.Invoice).HasForeignKey(i => i.IdInvoice).IsRequired();
        }
    }
}

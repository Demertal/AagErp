using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.ToTable("warranties");
            builder.Property(w => w.Malfunction).IsRequired().HasMaxLength(256);
            builder.Property(w => w.Info).HasMaxLength(256);
            builder.Property(w => w.DateReceipt).HasColumnType("date");
            builder.Property(w => w.DateDeparture).HasColumnType("date");
            builder.Property(w => w.DateIssue).HasColumnType("date");
            builder.Ignore(w => w.Error);
            builder.HasKey(w => w.Id);
        }
    }
}
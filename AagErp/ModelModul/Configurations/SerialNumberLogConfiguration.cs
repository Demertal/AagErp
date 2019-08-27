using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class SerialNumberLogConfiguration : IEntityTypeConfiguration<SerialNumberLog>
    {
        public void Configure(EntityTypeBuilder<SerialNumberLog> builder)
        {
            builder.ToTable("serialNumberLogs");
            builder.HasKey(s => s.Id);
            builder.Ignore(s => s.Error);
        }
    }
}
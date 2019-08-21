using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PropertyProductConfiguration : IEntityTypeConfiguration<PropertyProduct>
    {
        public void Configure(EntityTypeBuilder<PropertyProduct> builder)
        {
            builder.ToTable("propertyProducts");
            builder.HasKey(p => p.Id);
        }
    }
}

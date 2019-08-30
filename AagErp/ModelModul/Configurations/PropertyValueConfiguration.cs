using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PropertyValueConfiguration : IEntityTypeConfiguration<PropertyValue>
    {
        public void Configure(EntityTypeBuilder<PropertyValue> builder)
        {
            builder.ToTable("propertyValues");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Value).IsRequired().HasMaxLength(50);
            builder.Ignore(p => p.Error);
            builder.HasMany(p => p.PropertyProductsCollection).WithOne(p => p.PropertyValue)
                .HasForeignKey(p => p.IdPropertyValue);
        }
    }
}

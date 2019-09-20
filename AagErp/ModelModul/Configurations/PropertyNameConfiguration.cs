using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class PropertyNameConfiguration : IEntityTypeConfiguration<PropertyName>
    {
        public void Configure(EntityTypeBuilder<PropertyName> builder)
        {
            builder.ToTable("propertyNames");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(20);
            builder.Ignore(c => c.ValidationRules);
            builder.HasMany(p => p.PropertyProductsCollection).WithOne(p => p.PropertyName).HasForeignKey(p => p.IdPropertyName);
            builder.HasMany(p => p.PropertyValuesCollection).WithOne(p => p.PropertyName).HasForeignKey(p => p.IdPropertyName).IsRequired();
        }
    }
}

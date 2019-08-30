using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(50);
            builder.Ignore(c => c.Error);
            builder.HasMany(c => c.ChildCategoriesCollection).WithOne(c => c.Parent).HasForeignKey(c => c.IdParent);
            builder.HasMany(c => c.ProductsCollection).WithOne(p => p.Category).HasForeignKey(p => p.IdCategory).IsRequired();
            builder.HasMany(c => c.PropertyNamesCollection).WithOne(p => p.Category).HasForeignKey(p => p.IdCategory);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class MovmentGoodTypeConfiguration : IEntityTypeConfiguration<MovmentGoodType>
    {
        public void Configure(EntityTypeBuilder<MovmentGoodType> builder)
        {
            builder.ToTable("movmentGoodTypes");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Code).IsRequired().HasMaxLength(20);
            builder.Property(m => m.Description).IsRequired().HasMaxLength(50);
            builder.HasMany(m => m.MovementGoodsCollection).WithOne(m => m.MovmentGoodType).HasForeignKey(m => m.IdType)
                .IsRequired();
        }
    }
}

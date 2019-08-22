using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class RevaluationProductConfiguration : IEntityTypeConfiguration<RevaluationProduct>
    {
        public void Configure(EntityTypeBuilder<RevaluationProduct> builder)
        {
            builder.ToTable("revaluationProducts");
            builder.HasKey(r => r.Id);
            builder.HasMany(r => r.PriceProducts).WithOne(p => p.RevaluationProduct)
                .HasForeignKey(p => p.IdRevaluation).IsRequired();
        }
    }
}
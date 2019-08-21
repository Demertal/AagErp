using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class RevaluationProductConfiguration : IEntityTypeConfiguration<RevaluationProducts>
    {
        public void Configure(EntityTypeBuilder<RevaluationProducts> builder)
        {
            builder.ToTable("revaluationProducts");
            builder.HasKey(r => r.Id);
            builder.HasMany(r => r.PriceProducts).WithOne(p => p.RevaluationProducts)
                .HasForeignKey(p => p.IdRevaluation).IsRequired();
        }
    }
}
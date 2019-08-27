﻿using Microsoft.EntityFrameworkCore;
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
            builder.Property(r => r.DateRevaluation).HasColumnType("datetime").IsRequired().HasDefaultValueSql("getdate()");
            builder.Ignore(r => r.Error);
            builder.HasMany(r => r.PriceProducts).WithOne(p => p.RevaluationProduct)
                .HasForeignKey(p => p.IdRevaluation).IsRequired();
        }
    }
}
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("stores");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Title).IsRequired().HasMaxLength(50);
            builder.Ignore(s => s.Error);
            builder.HasMany(s => s.ArrivalMovementGoodsReports).WithOne(m => m.ArrivalStore).HasForeignKey(m => m.IdArrivalStore);
            builder.HasMany(s => s.DisposalMovementGoodsReports).WithOne(m => m.DisposalStore).HasForeignKey(m => m.IdDisposalStore);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelModul.Models;

namespace ModelModul.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.Store).WithMany(c => c.UsersCollection).HasForeignKey(c => c.IdStore);
        }
    }
}
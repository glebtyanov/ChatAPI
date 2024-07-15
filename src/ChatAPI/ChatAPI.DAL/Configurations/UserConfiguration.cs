using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            builder.HasMany(x => x.Chats)
                .WithOne(x => x.Admin)
                .HasForeignKey(x => x.AdminId);
        }
    }
}
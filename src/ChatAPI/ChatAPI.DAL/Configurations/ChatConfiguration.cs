using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.DAL.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

            builder.HasOne(s => s.Admin)
                .WithMany(u => u.Chats)
                .HasForeignKey(s => s.AdminId)
                .IsRequired();
        }
    }
}
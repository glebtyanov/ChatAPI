using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAPI.DAL.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(s => s.Text).IsRequired().HasMaxLength(500);

            builder.HasOne(s => s.Chat)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.Chat)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
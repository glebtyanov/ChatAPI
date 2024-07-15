using System.Reflection;
using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.DAL.Data
{
    public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
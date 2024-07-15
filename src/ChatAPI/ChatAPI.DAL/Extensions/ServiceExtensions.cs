using ChatAPI.DAL.Data;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAPI.DAL.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IChatsRepository, ChatsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
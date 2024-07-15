using ChatAPI.BLL.Interfaces;
using ChatAPI.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAPI.BLL.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<IChatsService, ChatsService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services.AddScoped<IUsersService, UsersService>();
        }
    }
}
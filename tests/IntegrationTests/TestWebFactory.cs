using ChatAPI.DAL.Data;
using ChatAPI.PL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class TestWebFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ChatDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ChatDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTest")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>())
                {
                    appContext.Database.EnsureCreated();
                }

                services.AddSignalR();
            });
        }
    }
}
using ChatAPI.BLL.Extensions;
using ChatAPI.DAL.Extensions;
using ChatAPI.PL.Hubs;

namespace ChatAPI.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext(builder.Configuration);

            builder.Services.AddRepositories();
            builder.Services.AddBllServices();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            builder.Services.AddSignalR();
            builder.Services.AddScoped<ChatHub>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler();

            app.UseCors();
            app.MapControllers();

            app.MapHub<ChatHub>("chathub");

            app.Run();
        }
    }
}
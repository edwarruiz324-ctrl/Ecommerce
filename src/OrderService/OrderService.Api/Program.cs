namespace OrderService.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Models;
    using OrderService.Infrastructure;
    using OrderService.Application;

    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ecommerce - Order",
                    Version = "v1"
                });
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            if (!builder.Environment.IsEnvironment("Test"))
            {
                builder.Services.AddInfrastructure(builder.Configuration);
            }

            builder.Services.AddApplication();

            var app = builder.Build();

            app.UseRouting();

            // Middleware de errores global
            app.UseMiddleware<Common.Middleware.ExceptionMiddleware>();

            if (!builder.Environment.IsEnvironment("Test"))
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    OrderService.Infrastructure.Persistence.DbInitializer.Initialize(services);
                }
            }


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
                c.RoutePrefix = "swagger"; // Swagger en /swagger
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
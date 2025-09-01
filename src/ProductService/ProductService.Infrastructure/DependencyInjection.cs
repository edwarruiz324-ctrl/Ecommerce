namespace ProductService.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Common;
    using ProductService.Infrastructure.Persistence;
    using ProductService.Infrastructure.Repositories;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Falta configurar la conexion a BD");

            services.AddDbContext<ProductsDbContext>(options =>
                                      options.UseSqlServer(
                                        connectionString,
                                        b => b.MigrationsAssembly(typeof(ProductsDbContext).Assembly.FullName)
                                      ));

            // Repositorios + UoW
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

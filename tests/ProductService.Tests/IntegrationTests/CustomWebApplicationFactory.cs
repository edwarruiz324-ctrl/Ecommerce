namespace ProductService.Tests.IntegrationTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Common;
    using ProductService.Infrastructure.Persistence;
    using ProductService.Infrastructure.Repositories;
    using System.Linq;
    using ProductService.Api;

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Forzar el entorno a "Test"
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // 1) Remover registros previos del DbContext (SQL Server)
                // Eliminamos cualquier descriptor que registre opciones para ProductsDbContext
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<ProductsDbContext>) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>) &&
                     d.ServiceType.GenericTypeArguments[0] == typeof(ProductsDbContext))
                ).ToList();

                foreach (var d in descriptors)
                {
                    services.Remove(d);
                }

                // También por si hubieran registrado el DbContext directamente
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ProductsDbContext));
                if (dbContextDescriptor != null) 
                    services.Remove(dbContextDescriptor);

                // 2) Registrar in-memory
                services.AddDbContext<ProductsDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                // 3) Construir proveedor y crear BD en memoria
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
                    try
                    {
                        // Limpiamos por si quedó algo de otras pruebas
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed to ensure in-memory database was created: " + ex.Message, ex);
                    }
                }

                services.AddScoped<IProductRepository, ProductRepository>();

                services.AddScoped<IUnitOfWork, UnitOfWork>();            

            });
        }
    }   
}
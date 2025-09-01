namespace OrderService.Tests.IntegrationTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using OrderService.Application.Contracts;
    using OrderService.Application.Dtos;
    using OrderService.Domain.Repositories;
    using OrderService.Infrastructure;
    using OrderService.Infrastructure.Common;
    using OrderService.Infrastructure.Repositories;
    using System;
    using System.Linq;
    using OrderService.Api;

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Forzar el entorno a "Test"
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // 1) Remover registros previos del DbContext (SQL Server)
                // Eliminamos cualquier descriptor que registre opciones para OrderDbContext
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<OrderDbContext>) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>) &&
                     d.ServiceType.GenericTypeArguments[0] == typeof(OrderDbContext))
                ).ToList();

                foreach (var d in descriptors)
                {
                    services.Remove(d);
                }

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(OrderDbContext));
                
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                // 2) Registrar InMemory
                services.AddDbContext<OrderDbContext>(options =>
                    options.UseInMemoryDatabase("OrdersTestDb"));

                // 3) Crear BD en memoria
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
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

                // 4) Registrar Repositorios y UoW
                services.AddScoped<IOrderRepository, OrderRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                //services.AddScoped<IProductClient, ProductClient>();

                services.RemoveAll<IProductClient>();
                services.AddSingleton<IProductClient, FakeProductClient>();
            });
        }
    }

    public class FakeProductClient : IProductClient
    {
        private readonly Dictionary<int, ProductInfoDto?> _products;

        public FakeProductClient()
        {
            // Diccionario simulado de productos
            _products = new Dictionary<int, ProductInfoDto?>
            {
                {
                    1, new ProductInfoDto(1, "", 2, 50m)
                },
                {
                    2, new ProductInfoDto(2, "Mouse", 2, 50m)
                },
                {
                    3, new ProductInfoDto(3, "Mouse2", 2, -50m)
                },
                { 999, null }
            };
        }

        public Task<ProductInfoDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            _products.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }
    }
}

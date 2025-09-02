namespace ProductService.Tests.DependencyInjectionTests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Persistence;
    using ProductService.Infrastructure;

    public class DependencyInjectionTests
    {
        [Fact]
        public void AddInfrastructure_ShouldRegister_Core_Services()
        {
            // Arrange
            // Configuración en memoria que satisface AddInfrastructure
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;"},
                {"ProductService:BaseUrl", "https://localhost:5001"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();

            // Llamamos directamente a AddInfrastructure
            services.AddInfrastructure(configuration);

            var sp = services.BuildServiceProvider();

            // Assert - repos y UoW
            var repo = sp.GetService<IProductRepository>();
            var uow = sp.GetService<IUnitOfWork>();
            Assert.NotNull(repo);
            Assert.NotNull(uow);

            // Assert - DbContext
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetService<ProductsDbContext>();
            Assert.NotNull(ctx);
        }
    }
}

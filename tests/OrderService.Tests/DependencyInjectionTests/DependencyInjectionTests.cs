namespace OrderService.Tests.DependencyInjectionTests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using OrderService.Application.Contracts;
    using OrderService.Domain.Repositories;
    using OrderService.Infrastructure;
    using OrderService.Infrastructure.Repositories;

    public class DependencyInjectionTests
    {
        [Fact]
        public void AddInfrastructure_ShouldRegister_Core_Services()
        {
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

            var provider = services.BuildServiceProvider();

            // Verificamos que se puedan resolver
            var repo = provider.GetService<IOrderRepository>();
            var uow = provider.GetService<IUnitOfWork>();
            var productClient = provider.GetService<IProductClient>();
            var dbContext = provider.GetService<OrderDbContext>();

            Assert.NotNull(repo);
            Assert.NotNull(uow);
            Assert.NotNull(productClient);
            Assert.NotNull(dbContext);
        }
    }
}

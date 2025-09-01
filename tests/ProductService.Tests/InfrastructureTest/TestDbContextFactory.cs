namespace ProductsService.InfrastructureTest
{
    using Microsoft.EntityFrameworkCore;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Common;
    using ProductService.Infrastructure.Persistence;

    public class TestDbContextWrapper
    {
        public ProductsDbContext Context { get; }
        public IUnitOfWork UnitOfWork { get; }

        public TestDbContextWrapper(ProductsDbContext context)
        {
            Context = context;
            UnitOfWork = new UnitOfWork(context);
        }
    }

    public static class TestDbContextFactory
    {
        public static TestDbContextWrapper Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ProductsDbContext(options);
            return new TestDbContextWrapper(context);
        }
    }
}
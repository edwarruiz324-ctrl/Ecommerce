namespace OrderService.Tests.InfrastructureTest
{
    using Microsoft.EntityFrameworkCore;
    using OrderService.Domain.Repositories;
    using OrderService.Infrastructure;
    using OrderService.Infrastructure.Common;

    public class TestDbContextWrapper
    {
        public OrderDbContext Context { get; }
        public IUnitOfWork UnitOfWork { get; }

        public TestDbContextWrapper(OrderDbContext context)
        {
            Context = context;
            UnitOfWork = new UnitOfWork(context);
        }
    }

    public static class TestDbContextFactory
    {
        public static TestDbContextWrapper Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new OrderDbContext(options);
            return new TestDbContextWrapper(context);
        }
    }
}

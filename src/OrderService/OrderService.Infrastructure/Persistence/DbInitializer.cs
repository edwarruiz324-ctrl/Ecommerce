namespace OrderService.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OrderService.Domain.Entities;

    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            // Aplica migraciones pendientes
            context.Database.Migrate();


            if (!context.Orders.Any())
            {
                var orders = new[]
                {
                 new Order("123", new List<OrderItem>
                    {
                        new OrderItem(1,2,100m),
                        new OrderItem(2,1,150m)
                    }),
                 new Order("456", new List<OrderItem>
                    {
                        new OrderItem(4,2,100m),
                        new OrderItem(5,2,150m)
                    })
                };
            
                context.Orders.AddRange(orders);
                context.SaveChanges();
            };
            

        }
    }
}

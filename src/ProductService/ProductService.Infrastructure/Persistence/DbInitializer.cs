namespace ProductService.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using ProductService.Domain.Entities;

    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

            // Aplica migraciones pendientes
            context.Database.Migrate();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product("Laptop Gamer", "Laptop Gamer", 1500, 100),
                    new Product("Teclado Mecánico", "Teclado Mecánico", 100, 200),
                    new Product("Mouse Inalámbrico", "Mouse Inalámbrico", 50, 25),
                    new Product("Monitor Gamer", "Monitor Gamer", 50, 25)
                );
                context.SaveChanges();

            }
        }
    }
}

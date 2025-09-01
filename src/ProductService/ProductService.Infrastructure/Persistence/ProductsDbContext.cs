namespace ProductService.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using ProductService.Domain.Entities;

    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<Product>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("Products");
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.Description).HasMaxLength(1000);
                b.Property(p => p.Price).HasColumnType("decimal(18,2)");
                b.Property(p => p.Stock).IsRequired();
                b.Property(p => p.CreatedAt).IsRequired();
                b.Property(p => p.UpdatedAt).IsRequired();
            });

            /*modelBuilder.Entity<Product>().HasData(
                new
                {
                    Id = -1,
                    Name = "Laptop Gamer",
                    Description = "Laptop Gamer",
                    Price = 1500m,
                    Stock = 100,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new
                {
                    Id = -2,
                    Name = "Teclado Mecánico",
                    Description = "Laptop Gamer",
                    Price = 100m,
                    Stock = 200,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new
                {
                    Id = -3,
                    Name = "Mouse Inalámbrico",
                    Description = "Laptop Gamer",
                    Price = 50m,
                    Stock = 25,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new
                {
                    Id = -4,
                    Name = "Monitor Gamer",
                    Description = "Monitor Gamer",
                    Price = 50m,
                    Stock = 25,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );*/
        }
    }
}

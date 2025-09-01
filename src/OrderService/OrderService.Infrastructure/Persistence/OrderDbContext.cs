namespace OrderService.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using OrderService.Domain;
    using OrderService.Domain.Entities;
    using ProductService.Domain.Entities;

    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<Order>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.OrderDate = utcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerId).IsRequired();
                entity.Property(o => o.Status).IsRequired();
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.OrderDate).IsRequired();
                entity.HasMany(o => o.Items)
                      .WithOne()
                      .HasForeignKey("OrderId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            });

            // Datos iniciales
            /*modelBuilder.Entity<Order>().HasData(
                new 
                {
                    Id = -1,
                    CustomerId = "123",
                    Status = OrderStatus.Pending,
                    TotalAmount = 350m, 
                    OrderDate = DateTime.UtcNow
                },
                new 
                {
                    Id = -2,
                    CustomerId = "456",
                    Status = OrderStatus.Pending,
                    TotalAmount = 350m, 
                    OrderDate = DateTime.UtcNow
                }
            );

            // Semilla de OrderItems (usamos los Ids negativos para no chocar con futuros inserts)
            modelBuilder.Entity<OrderItem>().HasData(
                new 
                {
                    Id = -1,
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 100m,
                    OrderId = -1
                },
                new 
                {
                    Id = -2,
                    ProductId = 2,
                    Quantity = 1,
                    UnitPrice = 150m,
                    OrderId = -1
                },
                new 
                {
                    Id = -3,
                    ProductId = 4,
                    Quantity = 2,
                    UnitPrice = 100m,
                    OrderId = -2
                },
                new 
                {
                    Id = -4,
                    ProductId = 5,
                    Quantity = 1,
                    UnitPrice = 150m,
                    OrderId = -2
                }
            );*/
        }
    }
}

namespace ProductService.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;
    using ProductService.Infrastructure.Persistence;

    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default) =>
            await _context.Products.AsNoTracking().ToListAsync(ct);

        public Task AddAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product, CancellationToken ct = default)
        {
            // Si necesitas que el context trackee la entidad, puedes Attach si viene detached
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Remove(product);
            return Task.CompletedTask;
        }
    }
}

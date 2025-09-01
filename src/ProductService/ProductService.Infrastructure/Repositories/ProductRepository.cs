namespace ProductService.Infrastructure.Repositories
{
    using global::Common.Extensions;
    using global::Common.Models;
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

        public async Task<IEnumerable<Product>> GetPaginateAsync(PaginationFilter filter, CancellationToken ct = default)
        {
             return await _context.Products.AsNoTracking()
                .OrderBy(p => p.Id)
                .ApplyPagination(filter)
                .ToListAsync(ct);
        }

        public Task AddAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            return await _context.Products.CountAsync();
        }
    }
}

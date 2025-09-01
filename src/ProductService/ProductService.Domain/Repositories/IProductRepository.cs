namespace ProductService.Domain.Repositories
{
    using Common.Models;
    using ProductService.Domain.Entities;

    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<Product>> GetPaginateAsync(PaginationFilter filter, CancellationToken ct = default);
        Task AddAsync(Product product, CancellationToken ct = default);
        Task UpdateAsync(Product product, CancellationToken ct = default);
        Task DeleteAsync(Product product, CancellationToken ct = default);
        Task<int> CountAsync(CancellationToken ct = default);
    }
}

namespace OrderService.Domain.Repositories
{
    using OrderService.Domain.Entities;

    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Order order, CancellationToken ct = default);
        Task UpdateAsync(Order order, CancellationToken ct = default);
        Task DeleteAsync(Order order, CancellationToken ct = default);
    }
}
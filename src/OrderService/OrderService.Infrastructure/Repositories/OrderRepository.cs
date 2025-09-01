
namespace OrderService.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using OrderService.Domain.Entities;
    using OrderService.Domain.Repositories;

    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.AddAsync(order);
            return Task.CompletedTask;
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Orders
                                 .Include(o => o.Items)
                                 .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Orders
                                 .Include(o => o.Items)
                                 .ToListAsync(ct);
        }

        public Task UpdateAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Remove(order);
            return Task.CompletedTask;
        }
    }
}
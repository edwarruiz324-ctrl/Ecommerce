namespace OrderService.Infrastructure.Common
{
    using Microsoft.EntityFrameworkCore.Storage;
    using OrderService.Domain.Repositories;

    internal class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _tx;
        public Transaction(IDbContextTransaction tx) => _tx = tx;

        public Task CommitAsync(CancellationToken cancellationToken = default) => _tx.CommitAsync(cancellationToken);

        public Task RollbackAsync(CancellationToken cancellationToken = default) => _tx.RollbackAsync(cancellationToken);

        public ValueTask DisposeAsync() => _tx.DisposeAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _db;
        public UnitOfWork(OrderDbContext db) => _db = db;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
            return new Transaction(tx);
        }
    }
}

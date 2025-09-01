using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Repositories
{
    public interface IUnitOfWork
    {
        /// <summary> Persiste los cambios pendientes en el UoW. </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary> Inicia una transacción atómica. Devuelve un token de transacción que puede commitear/rollback. </summary>
        Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}

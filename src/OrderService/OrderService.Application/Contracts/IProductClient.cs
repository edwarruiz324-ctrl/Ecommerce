namespace OrderService.Application.Contracts
{
    using OrderService.Application.Dtos;

    public interface IProductClient
    {
        /// <summary>Obtiene información del producto o null si no existe.</summary>
        Task<ProductInfoDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    }
}

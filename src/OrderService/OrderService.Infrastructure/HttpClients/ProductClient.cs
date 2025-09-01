namespace OrderService.Infrastructure.HttpClients
{
    using OrderService.Application.Dtos;
    using OrderService.Application.Contracts;
    using System.Net.Http.Json;

    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductInfoDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            return await response.Content.ReadFromJsonAsync<ProductInfoDto>(cancellationToken: cancellationToken);
        }
    }
}

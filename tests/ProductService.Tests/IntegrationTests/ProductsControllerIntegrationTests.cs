
namespace ProductService.Tests.IntegrationTests
{
    using Common;
    using ProductService.Application.Dtos;
    using System.Net.Http.Json;

    public class ProductsControllerIntegrationTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_And_GetById_ShouldWork()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                Stock = 5
            };

            // Act - POST
            var createResponse = await _client.PostAsJsonAsync("/api/products", request);

            ////if (!createResponse.IsSuccessStatusCode)
            ////{
            ////    var body = await createResponse.Content.ReadAsStringAsync();
            ////}
            Assert.True(createResponse.IsSuccessStatusCode,
                $"Create failed. Body: {await createResponse.Content.ReadAsStringAsync()}");

            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Act - GET by Id
            var getResponse = await _client.GetAsync($"/api/products/{created.Id}");
            Assert.True(getResponse.IsSuccessStatusCode,
                $"Get failed. Body: {await getResponse.Content.ReadAsStringAsync()}");

            var fetched = await getResponse.Content.ReadFromJsonAsync<ProductDto>();

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal("Test Product", fetched!.Name);
        }

        [Fact]
        public async Task GetAll_ShouldReturnProducts()
        {
            // Arrange - Crear un producto
            var request = new CreateProductRequest
            {
                Name = "Another Product",
                Description = "Another Description",
                Price = 19.99m,
                Stock = 10
            };

            await _client.PostAsJsonAsync("/api/products", request);

            // Act
            var response = await _client.GetAsync("/api/products");
            Assert.True(response.IsSuccessStatusCode,
                $"GetAll failed. Body: {await response.Content.ReadAsStringAsync()}");

            var products = await response.Content.ReadFromJsonAsync<PagedResult<ProductDto>>();

            // Assert
            Assert.NotNull(products);
            Assert.NotEmpty(products!.Items);
        }

        [Fact]
        public async Task Update_ShouldModifyProduct()
        {
            // Arrange - Crear un producto
            var createRequest = new CreateProductRequest
            {
                Name = "Update Test",
                Description = "Before update",
                Price = 15.99m,
                Stock = 3
            };

            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(created);

            // Act - PUT
            var updateRequest = new UpdateProductRequest
            {
                Id = created!.Id,
                Name = "Updated Name",
                Description = "After update",
                Price = 20.00m,
                Stock = 99
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/products/{created!.Id}", updateRequest);

            //////if (!updateResponse.IsSuccessStatusCode)
            //////{
            //////    var body = await updateResponse.Content.ReadAsStringAsync();
            //////}
            Assert.True(updateResponse.IsSuccessStatusCode,
                $"Update failed. Body: {await updateResponse.Content.ReadAsStringAsync()}");

            // Assert - GET para verificar cambios
            var getResponse = await _client.GetAsync($"/api/products/{created.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<ProductDto>();

            Assert.NotNull(updated);
            Assert.Equal("Updated Name", updated!.Name);
            Assert.Equal(99, updated.Stock);
        }

        [Fact]
        public async Task Delete_ShouldRemoveProduct()
        {
            // Arrange - Crear un producto
            var createRequest = new CreateProductRequest
            {
                Name = "Delete Test",
                Description = "Will be deleted",
                Price = 5.00m,
                Stock = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(created);

            // Act - DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/products/{created!.Id}");
            Assert.True(deleteResponse.IsSuccessStatusCode,
                $"Delete failed. Body: {await deleteResponse.Content.ReadAsStringAsync()}");

            // Assert - GET debe dar 404
            var getResponse = await _client.GetAsync($"/api/products/{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}

